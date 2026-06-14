# Frontend UI Contracts: Device List with Bootstrap and Status Filter

**Feature**: [spec.md](../spec.md)
**Date**: 2026-05-22
**Scope**: Frontend-only. No backend API changes. Consumes existing `GET /api/devices`.

---

## Consumed API Contract

### `GET /api/devices`

Source of truth: `specs/001-smart-home-mvp/contracts/openapi.yaml`

**Response** `200 OK`:
```json
[
  {
    "deviceId": "uuid",
    "externalId": "string",
    "name": "string",
    "sensorType": "string",
    "latestMetricType": "string | null",
    "latestMetricValue": "number | null",
    "latestEventTimeUtc": "ISO8601 | null",
    "status": "active | stale"
  }
]
```

**Status mapping applied by frontend**:

| API `status` | Frontend `DeviceStatus` |
|---|---|
| `"active"` | `"online"` |
| `"stale"` | `"offline"` |
| anything else | `"unknown"` |

**Error handling**:
- Non-2xx response → `UiSurfaceState { status: 'error', errorMessage: 'Unable to load devices.' }`
- Network failure (fetch throws) → same error state
- Empty array → `UiSurfaceState { status: 'empty', errorMessage: null }`

---

## Component Interface Contract

### `DeviceListComponent`

**Selector**: `app-device-list`  
**Route**: `/devices` (lazy-loaded)  
**No `@Input()` / `@Output()` bindings** — fully self-contained route component.

**Rendered surface states**:

| `uiState.status` | Rendered element |
|---|---|
| `loading` | Bootstrap spinner: `<div class="spinner-border">` |
| `ready` | Bootstrap table: `<table class="table table-hover">` with device rows |
| `empty` | Bootstrap alert: `<div class="alert alert-info">` — "No devices registered." |
| `error` | Bootstrap alert: `<div class="alert alert-danger">` — `uiState.errorMessage` |

**Filter control**:
- Bootstrap dropdown or button-group bound to `selectedStatus: StatusFilterOption`
- Options: `all`, `online`, `offline`, `unknown`
- Selecting a filter value updates `selectedStatus`; the `filteredDevices` getter re-evaluates synchronously
- No API call is made on filter change

**Device row** (one per `filteredDevices` item):

| Column | Source field | Notes |
|---|---|---|
| Name | `name` | Plain text |
| External ID | `externalId` | Monospace / code element |
| Type | `deviceType` | Plain text |
| Status | `status` | Bootstrap `<span class="badge">` with `badgeClass(status)` |

---

## Shell Component Contract

### `AppComponent`

**Template change**: Replace entire `app.component.html` with:
```html
<nav class="navbar navbar-expand-lg navbar-dark bg-dark">
  <div class="container-fluid">
    <a class="navbar-brand" routerLink="/devices">Smart Home Portal</a>
  </div>
</nav>
<main class="container mt-4">
  <router-outlet />
</main>
```

`app.component.css` cleared of all placeholder rules.

---

## No New Backend Contracts

This feature introduces no new API endpoints, no schema changes, and no breaking changes to existing contracts. The only backend-touching change is the `normalizeStatus()` bug fix in `DevicePagesFacade`, which corrects the mapping of API `"active"` → `"online"` and `"stale"` → `"offline"`.
