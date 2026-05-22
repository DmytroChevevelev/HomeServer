import { environment } from '../../../../environments/environment';
import {
  DeviceDetailsHeaderViewModel,
  DeviceListItemViewModel,
  DeviceStatus,
  HistoricalTelemetryRowViewModel,
  UiSurfaceState
} from '../models/device-pages.models';
import { DateTimeRangeInput, validateDateTimeRange } from '../utils/date-time-filter.util';
import { mapCollectionState, mapDetailState } from './device-pages-state.mapper';
import { DevicesApiService } from './devices-api.service';

type FetchFn = (input: RequestInfo | URL, init?: RequestInit) => Promise<Response>;
const defaultFetch: FetchFn = (input, init) => globalThis.fetch(input, init);

interface DeviceApiDto {
  id?: string;
  deviceId?: string;
  externalId?: string;
  name?: string;
  sensorType?: string;
  type?: string;
  status?: string;
  registeredAtUtc?: string | null;
  isEnabled?: boolean;
  latestValue?: number | null;
  latestMetricValue?: number | null;
  lastUpdatedUtc?: string | null;
  latestEventTimeUtc?: string | null;
}

interface TelemetryApiDto {
  deviceId?: string;
  timestampUtc?: string;
  metricType?: string;
  value?: number;
}

export interface DeviceListResult {
  state: UiSurfaceState;
  items: DeviceListItemViewModel[];
}

export interface DeviceDetailsResult {
  state: UiSurfaceState;
  item: DeviceDetailsHeaderViewModel | null;
}

export interface DeviceHistoryResult {
  state: UiSurfaceState;
  items: HistoricalTelemetryRowViewModel[];
}

export interface DeviceUnregisterResult {
  state: UiSurfaceState;
}

export class DevicePagesFacade {
  constructor(
    private readonly devicesApi: DevicesApiService = new DevicesApiService(),
    private readonly fetchFn: FetchFn = defaultFetch,
    private readonly apiBaseUrl: string = environment.apiBaseUrl
  ) {}

  async getDeviceList(): Promise<DeviceListResult> {
    try {
      const response = await this.devicesApi.list();
      if (!response.ok) {
        console.warn('[DevicePagesFacade.getDeviceList] Request returned non-OK status.', {
          status: response.status,
          statusText: response.statusText,
          apiBaseUrl: this.apiBaseUrl
        });
        return {
          state: mapCollectionState<DeviceListItemViewModel>(null, false, 'Unable to load devices.'),
          items: []
        };
      }

      const payload = (await response.json()) as DeviceApiDto[];
      const items = payload.map((dto) => this.mapDeviceListItem(dto));

      return {
        state: mapCollectionState(items, false),
        items
      };
    } catch (error) {
      console.error('[DevicePagesFacade.getDeviceList] Failed to load devices.', {
        apiBaseUrl: this.apiBaseUrl,
        error
      });
      return {
        state: mapCollectionState<DeviceListItemViewModel>(null, false, 'Unable to load devices.'),
        items: []
      };
    }
  }

  async getDeviceDetails(deviceId: string): Promise<DeviceDetailsResult> {
    const listResult = await this.getDeviceList();
    if (listResult.state.status === 'error') {
      return {
        state: listResult.state,
        item: null
      };
    }

    const selected = listResult.items.find((item) => item.deviceId === deviceId);
    const state = mapDetailState(selected, false, selected ? null : 'Device not found.');

    return {
      state,
      item: selected
        ? {
            deviceId: selected.deviceId,
            externalId: selected.externalId,
            name: selected.name,
            deviceType: selected.deviceType,
            status: selected.status,
            statusColor: selected.statusColor,
            lastUpdatedUtc: selected.latestEventTimeUtc
          }
        : null
    };
  }

  async getDeviceHistory(deviceId: string, filter: DateTimeRangeInput): Promise<DeviceHistoryResult> {
    const filterState = validateDateTimeRange(filter);
    if (!filterState.isValidRange) {
      return {
        state: { status: 'error', errorMessage: filterState.validationMessage },
        items: []
      };
    }

    try {
      const response = await this.fetchFn(`${this.apiBaseUrl}/telemetry/latest`);
      if (!response.ok) {
        console.warn('[DevicePagesFacade.getDeviceHistory] Request returned non-OK status.', {
          status: response.status,
          statusText: response.statusText,
          apiBaseUrl: this.apiBaseUrl
        });
        return {
          state: mapCollectionState<HistoricalTelemetryRowViewModel>(null, false, 'Unable to load telemetry history.'),
          items: []
        };
      }

      const payload = (await response.json()) as TelemetryApiDto[];
      const items = payload
        .filter((item) => (item.deviceId ?? '') === deviceId)
        .map((item) => {
          const metricValue = item.value ?? 0;
          return {
            timestampUtc: item.timestampUtc ?? '',
            metricType: item.metricType ?? 'unknown',
            metricValue,
            metricValueDisplay: `${metricValue}`
          };
        });

      return {
        state: mapCollectionState(items, false),
        items
      };
    } catch (error) {
      console.error('[DevicePagesFacade.getDeviceHistory] Failed to load telemetry history.', {
        apiBaseUrl: this.apiBaseUrl,
        error
      });
      return {
        state: mapCollectionState<HistoricalTelemetryRowViewModel>(null, false, 'Unable to load telemetry history.'),
        items: []
      };
    }
  }

  async unregisterDevice(deviceId: string): Promise<DeviceUnregisterResult> {
    try {
      const response = await this.devicesApi.unregister(deviceId);
      if (!response.ok) {
        console.warn('[DevicePagesFacade.unregisterDevice] Request returned non-OK status.', {
          status: response.status,
          statusText: response.statusText,
          apiBaseUrl: this.apiBaseUrl,
          deviceId
        });
        return {
          state: { status: 'error', errorMessage: 'Unable to unregister device.' }
        };
      }

      return {
        state: { status: 'ready', errorMessage: null }
      };
    } catch (error) {
      console.error('[DevicePagesFacade.unregisterDevice] Failed to unregister device.', {
        apiBaseUrl: this.apiBaseUrl,
        deviceId,
        error
      });
      return {
        state: { status: 'error', errorMessage: 'Unable to unregister device.' }
      };
    }
  }

  private mapDeviceListItem(dto: DeviceApiDto): DeviceListItemViewModel {
    const status = this.normalizeStatus(dto.status);
    const latestValue = dto.latestMetricValue ?? dto.latestValue ?? null;

    return {
      deviceId: dto.id ?? dto.deviceId ?? '',
      externalId: dto.externalId ?? '',
      name: dto.name ?? 'Unnamed device',
      deviceType: dto.sensorType ?? dto.type ?? 'unknown',
      status,
      statusColor: this.statusColor(status),
      registeredAtUtc: dto.registeredAtUtc ?? null,
      isEnabled: dto.isEnabled ?? true,
      latestValue,
      latestValueDisplay: latestValue === null ? 'No value' : `${latestValue}`,
      latestEventTimeUtc: dto.latestEventTimeUtc ?? dto.lastUpdatedUtc ?? null
    };
  }

  private normalizeStatus(value: string | undefined): DeviceStatus {
    const status = (value ?? '').toLowerCase();
    switch (status) {
      case 'active':
      case 'online':
        return 'online';
      case 'stale':
      case 'offline':
        return 'offline';
      case 'warning':
        return 'warning';
      default:
        return 'unknown';
    }
  }

  private statusColor(status: DeviceStatus): string {
    switch (status) {
      case 'online':
        return '#16a34a';
      case 'warning':
        return '#d97706';
      case 'offline':
        return '#dc2626';
      default:
        return '#6b7280';
    }
  }
}
