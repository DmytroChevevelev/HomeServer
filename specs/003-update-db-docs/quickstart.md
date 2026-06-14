# Quickstart: Database Sync and API Docs Metadata

## Prerequisites
- .NET SDK 9.x installed
- SQL Server instance reachable via `ConnectionStrings:DefaultConnection`
- Optional: EF Core CLI tools available (`dotnet tool install --global dotnet-ef`)

## Supported environment scope
- Local development machines (Windows/macOS/Linux)
- CI integration test runs
- Production migration orchestration is out of scope for this feature

## 1. Restore backend dependencies
```powershell
Set-Location backend/src/SmartHome.Api
dotnet restore
```

## 2. Create or update database schema
```powershell
dotnet ef database update
```
Expected result: migration history is applied and database schema is current.

## 2a. Verify data preservation on upgrade path
- Seed representative data in both `Devices` and `TelemetryReadings` with valid foreign-key linkage.
- Run `dotnet ef database update` again.
- Confirm seeded `Devices` and `TelemetryReadings` records remain accessible and relationally valid after upgrade.

## 3. Run API in development mode
```powershell
dotnet run
```
Expected result: API listens on `http://localhost:5151` and `https://localhost:5152`.

## 4. Verify documentation access contract endpoints
```powershell
Invoke-WebRequest http://localhost:5151/swagger | Select-Object -ExpandProperty StatusCode
Invoke-WebRequest http://localhost:5151/swagger/index.html | Select-Object -ExpandProperty StatusCode
Invoke-WebRequest http://localhost:5151/swagger/v1/swagger.json | Select-Object -ExpandProperty StatusCode
```
Expected result: status `200` in Development.

## 5. Verify modified endpoint metadata in OpenAPI JSON
- Open `http://localhost:5151/swagger/v1/swagger.json`.
- Confirm each modified endpoint includes:
  - `summary`
  - parameter descriptions when parameters are present
  - response entries with descriptions for expected status codes

## 6. Validate restricted docs mode behavior (policy check)
```powershell
$env:ASPNETCORE_ENVIRONMENT = "Production"
dotnet run
```
- Request `/swagger` and `/swagger/v1/swagger.json`.
- Confirm the endpoints are unavailable according to policy.

## 7. Run automated backend tests
```powershell
Set-Location ../../tests/SmartHome.Api.IntegrationTests
dotnet test
```
Expected result: contract and docs availability checks pass.

## 8. Optional schema regression check
```powershell
Set-Location ../../src/SmartHome.Api
dotnet ef migrations list
```
Expected result: expected migration list is present and current.

## 9. Migration generation rule
- Create a new EF migration only when model changes require schema changes.
- If no model changes exist, do not create an empty migration; only apply existing migrations.

## 10. Rollback notes
- Remove the latest unapplied migration with `dotnet ef migrations remove`.
- For applied migrations in local development, restore from backup or recreate the local database and rerun `pwsh scripts/apply-db-migrations.ps1`.

## 11. Recorded validation outcomes
- Integration suite result: `21 passed, 0 failed` from `dotnet test backend/tests/SmartHome.Api.IntegrationTests/SmartHome.Api.IntegrationTests.csproj`.
- Migration workflow result: `20260522153827_0002_DatabaseSync` created and applied successfully via `pwsh scripts/apply-db-migrations.ps1`.
- Documentation parity checks: Swagger routes and OpenAPI metadata tests pass in development mode with restricted-mode 404 behavior preserved.
