# Quickstart: Database Sync and API Docs Metadata

## Prerequisites
- .NET SDK 9.x installed
- SQL Server instance reachable via `ConnectionStrings:DefaultConnection`
- Optional: EF Core CLI tools available (`dotnet tool install --global dotnet-ef`)

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
