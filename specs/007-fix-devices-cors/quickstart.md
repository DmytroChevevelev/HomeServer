# Quickstart: Fix Devices CORS

## Goal
Ensure frontend device-list requests succeed from supported local development origins while maintaining strict allowlist behavior.

## Prerequisites
- Backend project: `backend/src/SmartHome.Api`
- Frontend project: `frontend`
- .NET SDK and Node.js installed

## Steps
1. Update `Cors:AllowedOrigins` in `backend/src/SmartHome.Api/appsettings.Development.json` to include:
   - `http://localhost:4200`
   - `http://localhost:4201`
   - `http://127.0.0.1:4200`
2. Start backend:
   - `dotnet run --project backend/src/SmartHome.Api/SmartHome.Api.csproj`
3. Start frontend on each supported origin and verify device list loads:
   - `npm run start` (default localhost:4200)
   - run alternate host/port launch as needed for 4201 and 127.0.0.1 checks
4. Run integration tests that validate origin behavior for `/api/devices`.

## Validation checklist
- Requests from each supported origin include `Access-Control-Allow-Origin`
- Request from unsupported origin excludes `Access-Control-Allow-Origin`
- `/api/devices` payload contract remains unchanged
- Swagger accessibility behavior remains unchanged

## Troubleshooting
- If Swagger works but frontend fails, inspect browser DevTools for CORS errors and compare request `Origin` to allowed origins list.
- Confirm backend was restarted after configuration changes.
