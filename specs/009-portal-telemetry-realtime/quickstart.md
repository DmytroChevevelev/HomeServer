# Quickstart: Portal Telemetry Realtime

## Goal
Validate that device list latest values update in real time and selected device details show telemetry list from a device-scoped endpoint.

## Prerequisites
- SQL Server Express available with migrated SmartHome database.
- Backend and frontend dependencies restored.
- Existing device and telemetry seed flow available (manual or simulator).

## Run
1. Start backend API.
2. Start frontend app.
3. Open portal devices page.
4. Start simulator or post telemetry to backend for a known device.

## Verify Real-Time Latest Value
1. Observe Latest Value column for the known device.
2. Send a new telemetry reading for that device.
3. Confirm value updates without manual page reload.
4. Confirm timestamp/status stays consistent with backend projection.

## Verify Selected Device Telemetry List
1. Open details for a selected device.
2. Confirm telemetry list rows match device telemetry contract fields.
3. Confirm ordering is newest first.
4. Confirm empty-state message appears for device with no readings.

## Verify Failure/Recovery
1. Simulate temporary telemetry list endpoint failure.
2. Confirm details page shows a clear error message and remains usable.
3. Restore API path and confirm list loads again.

## Minimum Automated Tests
- Backend integration:
  - GET /api/devices/{deviceId}/telemetry returns selected device rows only, ordered by EventTimeUtc descending.
  - Telemetry ingestion triggers notification publish for affected device.
- Frontend unit/component:
  - Device list updates Latest Value on sensor-change notification.
  - Device details renders telemetry list rows from contract shape.
  - Empty/error states render for selected-device telemetry retrieval.
