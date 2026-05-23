# Contracts: Portal Telemetry Realtime

## Endpoint: Get telemetry list for selected device
- Method: GET
- Route: /api/devices/{deviceId}/telemetry
- Purpose: Return telemetry records for one selected device.

### Path Parameters
- deviceId (guid, required): Device identifier.

### Query Parameters
- fromUtc (datetime, optional): Inclusive lower bound for EventTimeUtc.
- toUtc (datetime, optional): Inclusive upper bound for EventTimeUtc.
- limit (int, optional, default 100, max 500): Maximum number of rows.

### 200 Response
```json
[
  {
    "deviceId": "2f2f1b7b-ef48-4bc2-a96f-a6d058f6a4bd",
    "metricType": "temperature",
    "metricValue": 22.4,
    "eventTimeUtc": "2026-05-23T13:21:09Z"
  }
]
```

### Error Responses
- 400: Invalid deviceId/query parameters or invalid date range.
- 404: Device not found.
- 500: Unexpected failure while retrieving telemetry list.

## Real-time Notification Contract
- Transport: SignalR Hub
- Hub route: /hubs/telemetry
- Event name: sensorValueChanged

### Event Payload
```json
{
  "deviceId": "2f2f1b7b-ef48-4bc2-a96f-a6d058f6a4bd",
  "latestMetricValue": 22.4,
  "latestEventTimeUtc": "2026-05-23T13:21:09Z",
  "metricType": "temperature"
}
```

### Client behavior expectation
- Device list listens for sensorValueChanged and updates Latest Value for matching deviceId.
- Device details may refresh selected-device telemetry list when event deviceId equals selected device.

## Documentation metadata requirements
- New or modified endpoints must include OpenAPI summary, parameter descriptions, and response descriptions in endpoint mapping.
- Existing documentation access routes (/swagger, /swagger/index.html, /swagger/v1/swagger.json) remain unchanged and must continue to be documented per repository contract.
