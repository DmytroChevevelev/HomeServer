# Quickstart: Frontend Device Pages

## Prerequisites

- Backend API running locally.
- Frontend dependencies installed (`npm install` in `frontend`).

## 1. Start backend API

```powershell
Set-Location backend/src/SmartHome.Api
dotnet run
```

Expected outcome:
- API is available for frontend list/details data retrieval.

## 2. Start frontend application

```powershell
Set-Location frontend
npm run start
```

Expected outcome:
- Frontend launches at `http://localhost:4200`.

## 3. Verify main device list page

- Open main page route.
- Confirm each device row shows:
  - status indicator color
  - type
  - latest value or explicit no-value placeholder
- Confirm empty and loading states are meaningful when data is unavailable or pending.

## 4. Verify device row navigation

- Click any device row on main page.
- Confirm route changes to details page for that device.

## 5. Verify details page and history filter

- Confirm details header displays device identity information.
- Confirm historical list renders.
- Apply date-time range filter and verify list updates.
- Enter invalid range (start later than end) and verify validation guidance appears.

## 6. Verify add/register navigation

- From main page, click add/register device button.
- Confirm route navigates to registration page.

## 7. Test coverage checks

```powershell
Set-Location frontend
npm run test
```

Expected outcome:
- Route/component/service tests for list rendering, details navigation, filter behavior, and registration navigation pass.