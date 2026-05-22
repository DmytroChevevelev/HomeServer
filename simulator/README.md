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

## Commands and Runtime

The simulator runtime is command-driven and supports these commands:

- `register-device`
- `start-send-telemetry`
- `stop-send-telemetry`

Example command session:

```text
> register-device
register-device: success (201)

> start-send-telemetry
start-send-telemetry: started with interval 2s

> stop-send-telemetry
stop-send-telemetry: stopped
```

Telemetry profile files are resolved by device name convention. A profile should be placed at:

`profiles/<DeviceName>.json`

Where `<DeviceName>` comes from simulator configuration.

Telemetry profile file format example:

```json
{
	"metrics": [
		{
			"metricType": "temperature",
			"metricValue": 21.5
		},
		{
			"metricType": "humidity",
			"metricValue": 45.2
		}
	]
}
```

## Notes

- `start-send-telemetry` requires a valid device profile file matching `DeviceName`.
- If `SendIntervalSeconds` is absent, zero, or invalid, the simulator falls back to 2 seconds.
