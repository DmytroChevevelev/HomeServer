# Device Management API Contract

## GET /api/devices

Returns the device list projection used by the Angular devices page.

### Success Response

- Status: `200 OK`
- Body: Array of device summary records
- Required fields per item:
  - `deviceId`
  - `externalId`
  - `name`
  - `sensorType`
  - `status`
  - `latestMetricType`
  - `latestMetricValue`
  - `latestEventTimeUtc`

### Error Responses

- `500 Internal Server Error` when the projection cannot be generated
- Error payloads must remain structured and suitable for frontend error handling

## POST /api/devices

Registers a new device.

### Request Body

- Must include the backend-required device contract fields
- Must be validated for missing/blank required values and duplicate external identifiers

### Success Response

- Status: `201 Created`
- Body: created device record

### Error Responses

- `400 Bad Request` for missing or invalid fields
- `409 Conflict` for duplicate external identifiers
- `500 Internal Server Error` for unexpected persistence or projection failures

## DELETE /api/devices/{deviceId}

Unregisters a device.

### Path Parameters

- `deviceId`: identifier of the device to remove

### Success Response

- Status: `200 OK` or `204 No Content` depending on backend convention
- Body: clear success confirmation if a body is returned

### Error Responses

- `404 Not Found` when the device does not exist
- `400 Bad Request` when the identifier is invalid
- `500 Internal Server Error` for unexpected removal failures

## Telemetry Projection Behavior

- The latest telemetry value shown in the UI is derived from persisted telemetry readings.
- The frontend should treat `latestMetricValue` and `latestEventTimeUtc` as the source of truth when rendering device summaries.
- Older legacy aliases may be tolerated for compatibility, but the backend contract remains authoritative.