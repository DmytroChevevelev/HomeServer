export type DeviceStatus = 'online' | 'offline' | 'warning' | 'unknown';

export type DevicePagesUiState = 'loading' | 'ready' | 'empty' | 'error';

export interface DeviceListItemViewModel {
  deviceId: string;
  externalId: string;
  name: string;
  deviceType: string;
  status: DeviceStatus;
  statusColor: string;
  latestValue: number | null;
  latestValueDisplay: string;
}

export interface DeviceDetailsHeaderViewModel {
  deviceId: string;
  externalId: string;
  name: string;
  deviceType: string;
  status: DeviceStatus;
  statusColor: string;
  lastUpdatedUtc: string | null;
}

export interface HistoricalTelemetryRowViewModel {
  timestampUtc: string;
  metricType: string;
  metricValue: number;
  metricValueDisplay: string;
}

export interface DeviceHistoryFilterState {
  fromUtc: string;
  toUtc: string;
  isValidRange: boolean;
  validationMessage: string;
}

export interface UiSurfaceState {
  status: DevicePagesUiState;
  errorMessage: string | null;
}

export type StatusFilterOption = DeviceStatus | 'all';
