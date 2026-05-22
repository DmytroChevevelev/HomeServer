# Development Commands

## Backend
- `dotnet restore backend/src/SmartHome.Api/SmartHome.Api.csproj`
- `dotnet build backend/src/SmartHome.Api/SmartHome.Api.csproj`
- `dotnet run --project backend/src/SmartHome.Api/SmartHome.Api.csproj`
- `Invoke-WebRequest http://localhost:5151/swagger/v1/swagger.json | Select-Object -ExpandProperty StatusCode`
- `Start-Process http://localhost:5151/swagger`

## Frontend
- `cd frontend`
- `npm install`
- `npm run start`

## Simulator
- `dotnet run --project simulator/src/SmartHome.Simulator/SmartHome.Simulator.csproj`

## Tests
- `dotnet test backend/tests/SmartHome.Api.UnitTests/SmartHome.Api.UnitTests.csproj`
- `dotnet test backend/tests/SmartHome.Api.IntegrationTests/SmartHome.Api.IntegrationTests.csproj`
