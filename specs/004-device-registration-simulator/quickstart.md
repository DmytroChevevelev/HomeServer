# Quickstart: Simulator Device Registration Commands

## Prerequisites

- .NET SDK 9.x installed.
- Backend API runnable from `backend/src/SmartHome.Api`.
- Simulator configuration populated in `simulator/src/SmartHome.Simulator/appsettings.json`.
- A device telemetry values file available for the configured device name.

## 1. Start the backend API

```powershell
Set-Location backend/src/SmartHome.Api
dotnet run
```

Expected outcome:
- The API starts locally and exposes `POST /api/devices` and `POST /api/telemetry`.

## 2. Configure the simulator device profile

Set these values in `simulator/src/SmartHome.Simulator/appsettings.json`:

- `ApiBaseUrl`: backend API base URL.
- `DeviceExternalId`: device identifier used for registration and telemetry.
- `DeviceName`: device name used for registration and telemetry file lookup.
- `SensorType`: sensor type used for registration.
- `SendIntervalSeconds`: optional positive override; omit or set invalid values to verify the 2-second default.

Create a telemetry values file whose base name matches the configured device name.

Expected outcome:
- The simulator can resolve configuration and locate the device-specific telemetry profile before telemetry starts.

## 3. Run the simulator

```powershell
Set-Location simulator/src/SmartHome.Simulator
dotnet run
```

Expected outcome:
- The simulator starts, remains interactive, and displays the supported commands.

## 4. Register the device

Enter:

```text
register-device
```

Expected outcome:
- The simulator sends a registration request to the backend and prints either success details or a clear validation/conflict/connectivity failure.

## 5. Start telemetry

Enter:

```text
start-send-telemetry
```

Expected outcome:
- The simulator loads the device-specific telemetry values file.
- Telemetry requests begin at the configured interval, or every 2 seconds if no valid interval is configured.
- Starting telemetry again while already active returns a clear no-op or invalid-state message.

## 6. Stop telemetry

Enter:

```text
stop-send-telemetry
```

Expected outcome:
- The active send loop stops without exiting the simulator process.
- The simulator remains available for more commands or `Ctrl+C` shutdown.

## 7. Validate failure paths

Run these focused checks:

- Remove or rename the device-specific telemetry values file, then run `start-send-telemetry` and confirm the simulator blocks startup with an actionable error.
- Set `SendIntervalSeconds` to `0` or a non-numeric value, then confirm telemetry uses the default 2-second cadence.
- Run `stop-send-telemetry` before starting and confirm the simulator reports there is no active send loop.
- Run `register-device` twice and confirm duplicate-device feedback is clear.

## Validation Results (2026-05-22)

- Simulator tests: `dotnet test simulator/tests/SmartHome.Simulator.Tests/SmartHome.Simulator.Tests.csproj`
	- Result: 18 total, 18 passed, 0 failed.
- Backend integration tests: `dotnet test backend/tests/SmartHome.Api.IntegrationTests/SmartHome.Api.IntegrationTests.csproj`
	- Result: 21 total, 21 passed, 0 failed.
	- Notes: Build emitted existing XML documentation warnings in backend project; no functional test failures.