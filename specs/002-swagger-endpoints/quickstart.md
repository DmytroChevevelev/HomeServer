# Quickstart: Backend Swagger Endpoint Discovery

## Prerequisites
- .NET SDK 9.x installed
- Local backend dependencies configured (connection string and allowed origins)

## 1. Restore and run backend
```powershell
Set-Location backend/src/SmartHome.Api
dotnet restore
dotnet run
```

## 2. Open documentation UI
- Open http://localhost:5151/swagger in a browser.
- Verify that API groups and endpoints are visible.

## 3. Verify machine-readable contract
```powershell
Invoke-WebRequest http://localhost:5151/swagger/v1/swagger.json | Select-Object -ExpandProperty StatusCode
```
Expected result: `200` in development mode.

## 4. Validate interactive execution
- In the docs UI, choose one endpoint (for example, `POST /api/devices`).
- Execute with a valid payload.
- Confirm response status and body are displayed.
- Execute the same endpoint with an invalid payload (for example, missing required fields).
- Confirm validation error status and response body are visible in the docs UI.

## 5. Validate restricted mode behavior
```powershell
$env:ASPNETCORE_ENVIRONMENT = "Production"
dotnet run
```
- Request docs endpoints again.
- Confirm docs UI and JSON contract are unavailable according to policy.

## 6. Run automated tests
```powershell
Set-Location ../../tests/SmartHome.Api.IntegrationTests
dotnet test
```
- Confirm contract/integration coverage for docs availability and retrieval passes.

## 7. Recorded validation outcomes
- Swagger UI is reachable at `/swagger/index.html` in development mode.
- OpenAPI JSON is reachable at `/swagger/v1/swagger.json` in development mode.
- Both docs endpoints return `404` in non-development mode by default.
- Contract tests verify required API paths and telemetry response documentation.
