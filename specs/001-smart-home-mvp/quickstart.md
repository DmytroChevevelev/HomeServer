# Quickstart: Smart Home MVP Telemetry Flow

## Prerequisites
- .NET 9 SDK
- Node.js LTS + Angular CLI
- SQL Server Express (local)

## 1) Configure backend
1. Set SQL connection string in backend app settings.
2. Configure allowed CORS origin to frontend URL (for example `http://localhost:4200`).
3. Set API listen URL (for example `http://localhost:5151`).

## 2) Configure frontend
1. Set Angular serve port (for example `4200`).
2. Set API base URL in environment config (for example `http://localhost:5151/api`).

## 3) Configure simulator
1. Set API base URL to backend telemetry endpoint.
2. Set send interval (for example 1-5 seconds).
3. Enable deterministic mode for repeatable tests.

## 4) Run schema and services
1. Run EF Core migrations to create/update database schema.
2. Start backend API: `dotnet run --project backend/src/SmartHome.Api/SmartHome.Api.csproj`
3. Start Angular frontend: `cd frontend && npm install && npm run start`
4. Start simulator telemetry producer: `dotnet run --project simulator/src/SmartHome.Simulator/SmartHome.Simulator.csproj`

## 5) Validate MVP slice
1. Register a device from UI or API.
2. Confirm the device appears in the dashboard list.
3. Start simulator for the registered device external id.
4. Confirm telemetry is accepted and latest values appear in dashboard.
5. Pause simulator for more than 5 minutes and verify status becomes `stale`.

## 6) Validate non-happy paths
1. Try duplicate device registration and expect structured validation error.
2. Send telemetry for unknown device and expect not found error.
3. Trigger burst traffic and verify throttled responses are structured (`429`) and system remains responsive.

## Testing expectations
- Unit tests for validation and status derivation logic.
- Integration tests for registration, ingestion, and latest telemetry query.
- Contract tests against `contracts/openapi.yaml`.

## Validation command

- Run `pwsh scripts/validate-mvp.ps1` for a guided MVP verification checklist.
