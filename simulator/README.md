# Smart Home Simulator

Console application that publishes telemetry events to the Smart Home backend API for local testing and demo scenarios.

## Prerequisites

- .NET SDK 9.x
- Backend API running locally
- Simulator configuration in `appsettings.json`

## Run

```powershell
Set-Location simulator/src/SmartHome.Simulator
dotnet run
```

## Configuration

The simulator reads these settings from `appsettings.json`:

- `ApiBaseUrl` - backend base URL, defaults to `http://localhost:5151/api`
- `SendIntervalSeconds` - delay between telemetry submissions, defaults to `2`
- `DeviceExternalId` - device identifier used for telemetry, defaults to `device-001`

## Behavior

- Publishes telemetry until cancelled.
- Uses `Ctrl+C` to stop gracefully.
- Assumes the target device already exists in the backend.

## TODO:
- implement device registration behavior.
    - console gets command from input and send request to the backend
    - there are three command: "register-device", "start-send-telemetry" and "stop-send-telemetry"
    - configuration should allow to configure an interval between telemetry data. By default - 2sec.
    - values for telemetry should come from additional file with the same name as device name