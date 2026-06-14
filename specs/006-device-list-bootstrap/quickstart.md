# Developer Quickstart: Device List with Bootstrap and Status Filter

**Feature branch**: `006-device-list-bootstrap`

---

## Prerequisites

- Node.js (any LTS) installed
- .NET 9 SDK installed
- SQL Server Express running locally (or connection string configured)
- Angular CLI 19 available (`npx ng` works if not global)

---

## 1. Install Bootstrap

```powershell
cd frontend
npm install bootstrap
```

Verify `bootstrap` appears in `package.json` dependencies.

---

## 2. Register Bootstrap in angular.json

Add the Bootstrap CSS to the `styles` array inside `angular.json` → `projects.smart-home-portal-frontend.architect.build.options.styles`:

```json
"styles": [
  "node_modules/bootstrap/dist/css/bootstrap.min.css",
  "src/styles.css"
]
```

---

## 3. Run the Application

Start backend:
```powershell
cd backend/src/SmartHome.Api
dotnet run
# API available at http://localhost:5151
```

Start frontend:
```powershell
cd frontend
npm run start
# App available at http://localhost:4200
```

Navigate to `http://localhost:4200` — you should see the Bootstrap navbar and the device list.

---

## 4. Run Frontend Tests

```powershell
cd frontend
npx ng test --watch=false --browsers=ChromeHeadless --include='src/app/features/devices/**/*.spec.ts'
```

Expected: all tests pass including the new `device-list.component.spec.ts` tests.

---

## 5. Verify the Feature

With at least one device registered in the backend:

1. Open `http://localhost:4200/devices`
2. Confirm the Angular default template is gone — only the Bootstrap navbar and device table are visible
3. Confirm device rows display name, external ID, type, and a Bootstrap status badge
4. Select a status filter (e.g., "Offline") — verify only matching devices are shown
5. Select "All" — verify all devices return

If no devices exist, register one:
```bash
curl -X POST http://localhost:5151/api/devices \
  -H "Content-Type: application/json" \
  -d '{"externalId":"test-001","name":"Test Sensor","sensorType":"temperature"}'
```

---

## 6. Build for Production

```powershell
cd frontend
npm run build
```

Verify build completes with no errors and Bootstrap CSS is included in `dist/`.

---

## Key Files Changed

| File | Change |
|---|---|
| `frontend/package.json` | Bootstrap dependency added |
| `frontend/angular.json` | Bootstrap CSS in styles array |
| `frontend/src/app/app.component.html` | Replaced with Bootstrap navbar + router-outlet |
| `frontend/src/app/app.component.css` | Cleared of placeholder styles |
| `frontend/src/app/features/devices/models/device-pages.models.ts` | `StatusFilterOption` type added |
| `frontend/src/app/features/devices/services/device-pages.facade.ts` | `normalizeStatus()` fixed for `active`/`stale` |
| `frontend/src/app/features/devices/device-list/device-list.component.ts` | Fully implemented with filter state |
| `frontend/src/app/features/devices/device-list/device-list.component.html` | Bootstrap table, badges, filter, states |
| `frontend/src/app/features/devices/device-list/device-list.component.spec.ts` | New unit tests |
