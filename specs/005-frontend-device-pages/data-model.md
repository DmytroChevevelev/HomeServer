# Data Model: Frontend Device Pages

## DeviceListItemViewModel

- Purpose: Represents one device row on the main page list.
- Fields:
  - `deviceId`: unique identifier used for navigation.
  - `externalId`: operator-visible device reference.
  - `name`: device display name.
  - `deviceType`: sensor/type label.
  - `status`: normalized status value used by color indicator.
  - `statusColor`: resolved UI color token/class for status.
  - `latestValue`: most recent value if available.
  - `latestValueDisplay`: formatted value text or no-value placeholder.
- Validation:
  - `status` must resolve to a known indicator state or fallback `unknown`.
  - `latestValueDisplay` must never be empty; it must show either value or placeholder.

## DeviceDetailsHeaderViewModel

- Purpose: Header content shown on the device details page for selected device context.
- Fields:
  - `deviceId`
  - `externalId`
  - `name`
  - `deviceType`
  - `status`
  - `statusColor`
  - `lastUpdatedUtc`
- Validation:
  - `deviceId` and `name` are required to render details context.

## HistoricalTelemetryRowViewModel

- Purpose: One entry in the details page historical list.
- Fields:
  - `timestampUtc`
  - `metricType`
  - `metricValue`
  - `metricValueDisplay`
- Validation:
  - `timestampUtc` must be parseable as date-time.
  - `metricType` must be non-empty.

## DeviceHistoryFilterState

- Purpose: Date-time filter state applied to historical telemetry records.
- Fields:
  - `fromUtc`
  - `toUtc`
  - `isValidRange`
  - `validationMessage`
- Validation:
  - `isValidRange` is true only when both values are valid and `fromUtc <= toUtc`.
  - Invalid range prevents filter apply action and shows `validationMessage`.

## DevicePagesUiState

- Purpose: Shared UI state semantics for list/details data retrieval.
- States:
  - `loading`
  - `ready`
  - `empty`
  - `error`
- Validation:
  - Every data surface (list, details header, history list) must map to one of these states.