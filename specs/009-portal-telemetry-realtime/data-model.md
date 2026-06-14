# Data Model: Portal Telemetry Realtime

## Entity: Device
- Purpose: Represents a registered sensor source shown in the portal list and details page.
- Key fields:
  - Id (Guid)
  - ExternalId (string, unique)
  - Name (string)
  - SensorType (string)
  - RegisteredAtUtc (DateTime)
  - IsEnabled (bool)
- Relationships:
  - One-to-many with TelemetryReading (Device.Id -> TelemetryReading.DeviceId).

## Entity: TelemetryReading
- Purpose: Stores one measured value emitted by a device.
- Key fields:
  - Id (Guid)
  - DeviceId (Guid, FK)
  - MetricType (string)
  - MetricValue (decimal)
  - EventTimeUtc (DateTime)
- Validation rules:
  - DeviceId must reference existing Device.
  - MetricType required, non-empty.
  - EventTimeUtc required.

## Projection: DeviceLatestTelemetry
- Purpose: Drives Latest Value column in device list.
- Fields:
  - DeviceId
  - LatestMetricValue (decimal | null)
  - LatestEventTimeUtc (DateTime | null)
  - Status (derived)
- State transitions:
  - On telemetry ingest: recompute latest projection for affected device.
  - On no readings: projection remains null latest fields.

## DTO: DeviceTelemetryListItem
- Purpose: Contract row for selected-device telemetry list endpoint.
- Fields:
  - DeviceId (Guid)
  - MetricType (string)
  - MetricValue (decimal)
  - EventTimeUtc (DateTime)
- Ordering:
  - Descending by EventTimeUtc (newest first).

## Event Contract: SensorValueChanged
- Purpose: Notification payload from backend to frontend for in-session updates.
- Fields:
  - DeviceId (Guid)
  - LatestMetricValue (decimal | null)
  - LatestEventTimeUtc (DateTime | null)
  - MetricType (string)
- Behavior:
  - Published after successful telemetry ingestion persistence.
  - Frontend uses DeviceId to refresh or patch the corresponding list row.
