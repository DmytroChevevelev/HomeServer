# Development Commands

## Backend
- `dotnet restore backend/src/SmartHome.Api/SmartHome.Api.csproj`
- `dotnet build backend/src/SmartHome.Api/SmartHome.Api.csproj`
- `dotnet run --project backend/src/SmartHome.Api/SmartHome.Api.csproj`
- `dotnet ef database update --project backend/src/SmartHome.Api/SmartHome.Api.csproj --startup-project backend/src/SmartHome.Api/SmartHome.Api.csproj`
- `dotnet ef migrations list --project backend/src/SmartHome.Api/SmartHome.Api.csproj --startup-project backend/src/SmartHome.Api/SmartHome.Api.csproj`
- `Invoke-WebRequest http://localhost:5151/swagger/v1/swagger.json | Select-Object -ExpandProperty StatusCode`
- `Start-Process http://localhost:5151/swagger`

## Frontend
- `cd frontend`
- `npm install`
- `npm run start`
- `npm install bootstrap` *(first-time setup: Bootstrap 5 is a required dependency)*

### Frontend Tests
- `npx ng test --watch=false --browsers=ChromeHeadless` — run all frontend unit tests
- `npx ng test --watch=false --browsers=ChromeHeadless --include='src/app/features/devices/**/*.spec.ts'` — run device feature tests only

### Device List Status Filter
The device list at `/devices` provides a client-side status filter (All / Online / Offline / Unknown).
Filtering is performed in-memory — no additional API calls are made when changing the filter selection.
API status values `active` → `online` and `stale` → `offline` are mapped in `DevicePagesFacade.normalizeStatus()`.

## Simulator
- `dotnet run --project simulator/src/SmartHome.Simulator/SmartHome.Simulator.csproj`

## Tests
- `dotnet test backend/tests/SmartHome.Api.UnitTests/SmartHome.Api.UnitTests.csproj`
- `dotnet test backend/tests/SmartHome.Api.IntegrationTests/SmartHome.Api.IntegrationTests.csproj`

## Validation
- `pwsh scripts/apply-db-migrations.ps1`
- `pwsh scripts/validate-mvp.ps1`

## CORS Troubleshooting (`/api/devices`)
- Supported local frontend origins (development default):
	- `http://localhost:4200`
	- `http://localhost:4201`
	- `http://127.0.0.1:4200`
- Quick check for allowed origin header:
	- `Invoke-WebRequest -Uri http://localhost:5151/api/devices -Headers @{ Origin = 'http://localhost:4201' } | Select-Object -ExpandProperty Headers`
- If Swagger works but frontend fails:
	- Confirm frontend origin matches one of the supported origins above.
	- Confirm backend was restarted after changes to `appsettings.Development.json`.
	- Check browser DevTools for CORS error details and origin value.
