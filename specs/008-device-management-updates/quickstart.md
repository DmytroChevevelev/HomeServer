# Quickstart: Device Management Updates

## Prerequisites

- .NET SDK 9.x
- Node.js installed for the Angular frontend
- SQL Server Express available locally
- Backend, frontend, and simulator projects opened in the workspace

## Validate the backend contract

1. Start the backend API.
2. Open `/swagger` and confirm the devices endpoints are present.
3. Call `GET /api/devices` and verify the payload includes `latestMetricValue` and `latestEventTimeUtc`.
4. Register a device with `POST /api/devices` and confirm validation failures are returned clearly.
5. Exercise the unregister endpoint and confirm the target device is removed.

## Validate the frontend list and details flows

1. Start the Angular app.
2. Open the devices page and confirm each row shows summary data and the latest telemetry value.
3. Expand a row and confirm the additional metadata is visible.
4. Click a device name and confirm the details page opens for that device.
5. Trigger an unregister action and confirm the UI handles success and failure messages.

## Validate telemetry flow

1. Start the simulator or otherwise ingest telemetry for an existing device.
2. Reload the devices page and confirm the latest telemetry value changes in the visible row.
3. Open the device details page and confirm telemetry data reflects the latest backend projection.

## Validate observability

1. Force a frontend failure such as a rejected fetch or a non-OK response.
2. Confirm the browser console shows the operation name and API base URL.
3. Force a backend failure path and confirm structured logs are emitted through Serilog.

## Expected Outcomes

- Devices list displays summary and latest telemetry.
- Device details page opens from a device name click.
- Device unregister action succeeds or fails with clear feedback.
- Registration submits the full contract.
- Telemetry updates appear after refresh/reload.