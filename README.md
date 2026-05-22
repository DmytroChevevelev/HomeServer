# Agentic Engineering

Smart Home Portal is a local-first full-stack sample composed of a .NET 9 backend API, an Angular frontend, and a .NET simulator that publishes telemetry into the backend.

## Projects

- [backend](backend/README.md) - ASP.NET Core Web API, EF Core, SQL Server, and Swagger/OpenAPI support.
- [frontend](frontend/README.md) - Angular UI for the Smart Home portal.
- [simulator](simulator/README.md) - Console app that publishes telemetry to the backend API.

## Prerequisites

- .NET SDK 9.x
- Node.js 18+ and npm
- SQL Server Express or a compatible SQL Server instance for local development
- A browser for Swagger UI and frontend verification

## Common Workflows

### Backend

```powershell
Set-Location backend/src/SmartHome.Api
dotnet restore
dotnet build
dotnet run
```

### Frontend

```powershell
Set-Location frontend
npm install
npm run start
```

### Simulator

```powershell
Set-Location simulator/src/SmartHome.Simulator
dotnet run
```

### Tests

```powershell
dotnet test backend/tests/SmartHome.Api.UnitTests/SmartHome.Api.UnitTests.csproj
dotnet test backend/tests/SmartHome.Api.IntegrationTests/SmartHome.Api.IntegrationTests.csproj
```

## Documentation

- [Development commands](docs/development-commands.md)
- [MVP readiness checklist](docs/mvp-readiness.md)
- [Swagger feature specification](specs/002-swagger-endpoints/spec.md)
- [Swagger implementation plan](specs/002-swagger-endpoints/plan.md)
- [Swagger task list](specs/002-swagger-endpoints/tasks.md)
- [DB sync and docs spec](specs/003-update-db-docs/spec.md)
- [DB sync and docs plan](specs/003-update-db-docs/plan.md)
- [DB sync and docs tasks](specs/003-update-db-docs/tasks.md)
- [Device management updates spec](specs/008-device-management-updates/spec.md)
- [Device management updates plan](specs/008-device-management-updates/plan.md)
- [Device management updates tasks](specs/008-device-management-updates/tasks.md)

## Notes

- The backend exposes Swagger UI in development mode at `/swagger`.
- Database schema updates are operator-run using EF Core migration commands.
- The current feature branch is `008-device-management-updates`.
- Browser API consumers must use an origin included in backend `Cors:AllowedOrigins`; otherwise browser calls (for example `/api/devices`) are blocked by CORS even when endpoint health is OK.

## Device Management Workflow

1. Register a device from the frontend page at `/devices/register` (or `POST /api/devices`).
2. Open `/devices` and verify the latest telemetry value appears in each visible row.
3. Expand a row to view metadata such as registration time and enabled state.
4. Click a device name to open `/devices/:deviceId` details and telemetry.
5. Unregister a device from the details page (or `DELETE /api/devices/{deviceId}`).

## Logging Expectations

- Backend uses Serilog structured logs for registration, unregister, telemetry ingestion, and projection flows.
- Frontend logs operational failures to the browser console with operation context.
