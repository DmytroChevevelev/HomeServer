# Smart Home API

ASP.NET Core Web API for the Smart Home Portal backend. It provides device registration, telemetry ingestion, latest telemetry queries, and Swagger/OpenAPI documentation for local development.

## Highlights

- .NET 9 minimal API
- EF Core 9 with SQL Server
- Structured validation and error responses
- Swagger UI and OpenAPI JSON in development mode

## Prerequisites

- .NET SDK 9.x
- SQL Server Express or another SQL Server instance
- Connection string configured in `appsettings.Development.json` or environment-specific settings

## Run Locally

```powershell
Set-Location backend/src/SmartHome.Api
dotnet restore
dotnet run
```

The launch profile starts the API in `Development` and binds to `http://localhost:5151` by default (with HTTPS on `https://localhost:5152`).

## Swagger

- Swagger UI: `http://localhost:5151/swagger`
- OpenAPI JSON: `http://localhost:5151/swagger/v1/swagger.json`

Swagger is enabled in development and blocked by default in non-development environments.

## Tests

```powershell
Set-Location backend/tests/SmartHome.Api.UnitTests
dotnet test

Set-Location ../SmartHome.Api.IntegrationTests
dotnet test
```

## Project Structure

- `api/contracts` - request and response DTOs
- `api/endpoints` - route mappings
- `api/errors` - validation and error helpers
- `api/middleware` - request correlation and pipeline middleware
- `infrastructure` - database context and repositories
- `services` - domain and query services
