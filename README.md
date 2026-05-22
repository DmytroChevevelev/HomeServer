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

## Notes

- The backend exposes Swagger UI in development mode at `/swagger`.
- The current feature branch is `002-swagger-endpoints`.
