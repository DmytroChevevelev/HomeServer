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

interface DeviceApiDto {
  id?: string;
  deviceId?: string;
  externalId?: string;
  name?: string;
  sensorType?: string;
  type?: string;
  status?: string;
  latestValue?: number | null;
  lastUpdatedUtc?: string | null;
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

export class DevicePagesFacade {
  constructor(
    private readonly devicesApi: DevicesApiService = new DevicesApiService(),
    private readonly fetchFn: FetchFn = fetch,
    private readonly apiBaseUrl: string = environment.apiBaseUrl
  ) {}

  async getDeviceList(): Promise<DeviceListResult> {
    try {
      const response = await this.devicesApi.list();
      if (!response.ok) {
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
    } catch {
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
            lastUpdatedUtc: null
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
    } catch {
      return {
        state: mapCollectionState<HistoricalTelemetryRowViewModel>(null, false, 'Unable to load telemetry history.'),
        items: []
      };
    }
  }

  private mapDeviceListItem(dto: DeviceApiDto): DeviceListItemViewModel {
    const status = this.normalizeStatus(dto.status);
    const latestValue = dto.latestValue ?? null;

    return {
      deviceId: dto.id ?? dto.deviceId ?? '',
      externalId: dto.externalId ?? '',
      name: dto.name ?? 'Unnamed device',
      deviceType: dto.sensorType ?? dto.type ?? 'unknown',
      status,
      statusColor: this.statusColor(status),
      latestValue,
      latestValueDisplay: latestValue === null ? 'No value' : `${latestValue}`
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
