# Data Model: Smart Home MVP Telemetry Flow

## Entity: Device
- Description: Registered sensor endpoint shown in dashboard and used as telemetry parent.
- Fields:
  - `id` (uuid, primary key)
  - `externalId` (string, required, unique)
  - `name` (string, required, max 120)
  - `sensorType` (enum: temperature|humidity|motion|custom, required)
  - `registeredAtUtc` (datetime, required)
  - `isEnabled` (boolean, required, default true)
- Validation rules:
  - `externalId` must be unique and non-empty.
  - `name` must be non-empty after trimming.
- Relationships:
  - One `Device` to many `TelemetryReading`.

## Entity: TelemetryReading
- Description: Time-based measurement event submitted by simulator or external producer.
- Fields:
  - `id` (uuid, primary key)
  - `deviceId` (uuid, foreign key -> Device.id, required)
  - `metricType` (enum: temperature|humidity|motion|battery|custom, required)
  - `metricValue` (decimal, required)
  - `eventTimeUtc` (datetime, required)
  - `ingestedAtUtc` (datetime, required)
- Validation rules:
  - `deviceId` must reference an existing device.
  - `eventTimeUtc` cannot be null.
  - `metricValue` must be numeric and finite.
- Relationships:
  - Many `TelemetryReading` to one `Device`.

## Derived View: DeviceLatestTelemetry
- Description: Query projection used by dashboard to display latest values and status.
- Fields:
  - `deviceId`
  - `externalId`
  - `name`
  - `sensorType`
  - `latestMetricType` (nullable when no readings)
  - `latestMetricValue` (nullable when no readings)
  - `latestEventTimeUtc` (nullable when no readings)
  - `status` (`active`|`stale`)
- Status rule:
  - `active` when `latestEventTimeUtc >= nowUtc - 5 minutes`
  - `stale` otherwise (including no telemetry history)

## Error Contract Entity: ValidationError
- Description: Structured response for invalid or rejected API requests.
- Fields:
  - `code` (string)
  - `message` (string)
  - `field` (string, optional)
  - `requestId` (string)

## State Transitions
- Device lifecycle state (derived):
  - `stale` -> `active` when first valid telemetry arrives with event time within 5 minutes.
  - `active` -> `stale` when no telemetry event remains within the 5-minute window.
  - `stale` remains `stale` for never-reported devices.
