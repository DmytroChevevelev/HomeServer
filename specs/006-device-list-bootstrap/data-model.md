# Data Model: Device List with Bootstrap and Status Filter

**Feature**: [spec.md](spec.md)
**Date**: 2026-05-22

---

## Overview

This feature is entirely frontend-only. No backend schema, migration, or entity changes are required. All changes are limited to Angular view-models, component state, and UI rendering logic.

---

## Existing Frontend View Models (unchanged)

Located in `frontend/src/app/features/devices/models/device-pages.models.ts`.

### `DeviceStatus` (existing — no change)

```typescript
export type DeviceStatus = 'online' | 'offline' | 'warning' | 'unknown';
```

Used for badge coloring and filter matching. Note: `'warning'` is defined but cannot currently be sourced from the API (see research.md §2). Kept for forward compatibility.

### `DeviceListItemViewModel` (existing — no change)

| Field | Type | Description |
|---|---|---|
| `deviceId` | `string` | Internal UUID from API `DeviceId` field |
| `externalId` | `string` | Hardware-assigned external ID |
| `name` | `string` | Human-readable device name |
| `deviceType` | `string` | Sensor type (temperature, humidity, etc.) |
| `status` | `DeviceStatus` | Normalized operational status |
| `statusColor` | `string` | Hex color string for badge (legacy, see note) |
| `latestValue` | `number \| null` | Most recent telemetry value |
| `latestValueDisplay` | `string` | Formatted display string |

> **Note**: `statusColor` is a legacy hex string. This feature uses Bootstrap badge classes (`bg-success`, `bg-warning`, `bg-danger`, `bg-secondary`) derived from `status`, not `statusColor`. The `statusColor` field is preserved for compatibility.

### `UiSurfaceState` (existing — no change)

| Field | Type | Description |
|---|---|---|
| `status` | `'loading' \| 'ready' \| 'empty' \| 'error'` | UI rendering state |
| `errorMessage` | `string \| null` | Message to display in error alert |

---

## New / Modified Frontend State

### `StatusFilterOption` (new type — added to models file)

```typescript
export type StatusFilterOption = DeviceStatus | 'all';
```

Represents the selected value in the status filter control. `'all'` means no filter is applied. This is the only model addition required by this feature.

---

## Component State: `DeviceListComponent`

The `DeviceListComponent` gains the following private/public state fields as part of implementation:

| Field | Type | Default | Description |
|---|---|---|---|
| `allDevices` | `DeviceListItemViewModel[]` | `[]` | Full unfiltered list from facade |
| `uiState` | `UiSurfaceState` | `{ status: 'loading', errorMessage: null }` | Current render state |
| `selectedStatus` | `StatusFilterOption` | `'all'` | Currently selected filter |

**Derived (computed getter)**:

| Getter | Returns | Description |
|---|---|---|
| `filteredDevices` | `DeviceListItemViewModel[]` | `allDevices` filtered by `selectedStatus`; returns `allDevices` when `selectedStatus === 'all'` |

---

## Status Normalization Fix

The existing `normalizeStatus()` in `DevicePagesFacade` maps only `'online'`, `'offline'`, `'warning'`. The API returns `'active'` and `'stale'`. This must be corrected:

| API value | Mapped to |
|---|---|
| `active` | `online` |
| `stale` | `offline` |
| `online` | `online` (kept for future) |
| `offline` | `offline` (kept for future) |
| `warning` | `warning` (kept for future) |
| anything else | `unknown` |

---

## Bootstrap Badge Class Mapping

Used in the component template to set `class` on `<span class="badge">` elements:

| `DeviceStatus` | Bootstrap class |
|---|---|
| `online` | `bg-success` |
| `warning` | `bg-warning text-dark` |
| `offline` | `bg-danger` |
| `unknown` | `bg-secondary` |

This mapping is implemented as a method on the component (`badgeClass(status: DeviceStatus): string`) — not on the facade — to keep Bootstrap-specific concerns out of the data layer.

---

## No Backend Data Model Changes

- No EF Core migrations required.
- No new API endpoints.
- No changes to `Device`, `TelemetryReading`, or any backend entity.
