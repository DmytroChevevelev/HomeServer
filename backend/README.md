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

## Local CORS Origins

The backend uses an explicit allowlist from `Cors:AllowedOrigins` for browser requests.

Default development origins:
- `http://localhost:4200`
- `http://localhost:4201`
- `http://127.0.0.1:4200`

For browser requests to endpoints like `/api/devices`, the response includes
`Access-Control-Allow-Origin` only when the request origin matches the configured allowlist.

## Database Sync Workflow

Use operator-run EF Core commands to keep schema state current. Startup automatic migration execution is intentionally disabled.

```powershell
Set-Location backend/src/SmartHome.Api
dotnet ef migrations list
dotnet ef database update
```

If migration state cannot be evaluated at startup, the API logs a warning and continues serving requests.

## Endpoint Documentation Metadata

For every endpoint or middleware updated in this feature:

- OpenAPI `summary` and `description` must be present.
- Response descriptions must be present for success and failure status codes.
- Parameter/request property descriptions must be visible in generated OpenAPI schemas when inputs are present.

The baseline docs-access route behavior remains aligned with `specs/002-swagger-endpoints/contracts/openapi-docs.yaml`.

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
