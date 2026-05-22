# Data Model: Device Management Updates

## Device

Represents a registered smart-home device shown in the list and details experiences.

### Key Fields

- `deviceId`: Stable identifier used by the UI and routes.
- `externalId`: Device-side identifier used by the backend contract.
- `name`: Human-readable device name.
- `sensorType`: Device type/category used in telemetry processing.
- `status`: Current computed state used by the UI.
- `registeredAtUtc`: Timestamp of registration.
- `isEnabled`: Whether the device is active.

### Relationships

- A device has zero or more telemetry readings.
- A device may be removed through the unregister flow.

## TelemetryReading

Represents a timestamped measurement associated with a device.

### Key Fields

- `deviceId`: Foreign key to the owning device.
- `metricType`: Type/category of telemetry measurement.
- `metricValue`: Numeric value of the measurement.
- `eventTimeUtc`: Time the reading occurred.
- `ingestedAtUtc`: Time the backend stored the reading.

### Relationships

- Many telemetry readings belong to one device.
- The latest reading determines the visible latest telemetry value.

## DeviceSummary

Represents the device list projection consumed by the Angular list page.

### Key Fields

- `deviceId`
- `externalId`
- `name`
- `sensorType`
- `status`
- `latestMetricType`
- `latestMetricValue`
- `latestEventTimeUtc`

### Relationships

- Derived from `Device` plus the most recent `TelemetryReading`.

## DeviceDetailsView

Represents the details-page view model for a selected device.

### Key Fields

- Device profile fields used on the details page.
- Telemetry values used for the telemetry panel or history view.
- Unregister action state for the page.

### Relationships

- Derived from `Device` and the telemetry projection.

## DeviceRegistrationPayload

Represents the data required to create a complete device record.

### Key Fields

- `externalId`
- `name`
- `sensorType`
- Any additional contract fields required by the backend for full persistence

### Validation Rules

- Required fields must be present and non-empty.
- Duplicate external identifiers must be rejected.
- Payloads must match the backend contract exactly.

## DeviceUnregisterResult

Represents the backend response when removing a device.

### Key Fields

- `deviceId`
- `success`
- `message`

### Validation Rules

- The target device must exist.
- The UI should surface a clear failure reason when the remove operation cannot complete.