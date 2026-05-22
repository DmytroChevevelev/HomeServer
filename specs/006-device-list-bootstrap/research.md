# Research: Device List with Bootstrap and Status Filter

**Feature**: [spec.md](spec.md)
**Date**: 2026-05-22
**Phase**: 0 — Research & Unknown Resolution

---

## 1. Bootstrap Installation for Angular 19

**Decision**: Install `bootstrap` npm package and import via `angular.json` styles array.

**Rationale**: Angular 19 does not ship with Bootstrap. The standard non-SCSS approach is:
1. `npm install bootstrap` in `frontend/`
2. Add `"node_modules/bootstrap/dist/css/bootstrap.min.css"` to the `styles` array in `angular.json`

This avoids needing a Bootstrap Angular wrapper library (e.g., ng-bootstrap, ngx-bootstrap), which would add unnecessary complexity. Pure Bootstrap CSS classes are sufficient for table, badge, alert, button-group, and spinner components required by this feature.

**Alternatives considered**:
- `ng-bootstrap`: heavier, requires additional imports and Angular module setup. Rejected — overkill for table/badge/alert use.
- SCSS import via `styles.css`: viable but requires SASS toolchain already configured. Rejected — CSS bundle import is simpler and confirmed compatible with Angular CLI 19's esbuild builder.

---

## 2. Actual API Status Values vs. Spec

**Decision**: Map API status `"active"` → `DeviceStatus "online"`, `"stale"` → `DeviceStatus "offline"`. Filter UI provides: All, Online, Offline, Unknown. Remove `warning` from the filter (no API source currently).

**Rationale**: The backend `DeviceStatusEvaluator` only emits two values: `"active"` (telemetry within 5 minutes) and `"stale"` (older or absent). The existing `normalizeStatus()` in `DevicePagesFacade` has a bug — it only matches `"online" | "offline" | "warning"` against the raw API string, so `"active"` and `"stale"` both fall through to `"unknown"`. This must be fixed.

The spec's `warning` filter bucket cannot be populated from the current API; it is documented here and can be re-introduced when the backend exposes a `warning` status. For now the filter shows: **All | Online | Offline | Unknown**.

**Alternatives considered**:
- Keep `warning` filter option as a disabled/greyed-out placeholder: rejected — creates confusing empty-state in UI with no value.
- Add a backend `warning` status now: out of scope for this feature (no backend changes required per spec Constitution Alignment).

---

## 3. Client-Side Status Filtering Pattern in Angular 19

**Decision**: Hold the full unfiltered device list in a component field; apply a getter or computed signal to project the filtered view using `Array.filter()`. Use a `selectedStatus: DeviceStatus | 'all'` field bound to the filter control.

**Rationale**: Angular 19 standalone components with `CommonModule` already support `*ngFor` over a filtered array. No additional state management library (NgRx, etc.) is needed for a single-page list filter. A getter is preferred over a `pipe` to avoid creating a standalone filter pipe artifact that must be declared and tested separately.

**Alternatives considered**:
- `AsyncPipe` + RxJS `BehaviorSubject`: viable but unnecessary indirection for client-side filtering. Rejected — adds complexity without benefit for synchronous in-memory data.
- Angular `Signal`-based reactive state: acceptable but requires Angular 17+ signals pattern; kept as future improvement path. Rejected for this feature to minimize scope creep.

---

## 4. Angular Component Test Approach for Bootstrap UI

**Decision**: Use Jasmine/Karma unit tests with `TestBed.createComponent()`. Assert on rendered element counts (`querySelectorAll('tr')`) and class presence (`classList.contains('badge')`) without importing Bootstrap CSS in tests.

**Rationale**: Bootstrap CSS does not need to be loaded in Karma tests to verify structural correctness. Tests verify: (a) correct number of rows for given input, (b) filter changes update rendered row count, (c) error state shows alert element, (d) loading state shows spinner element.

**Alternatives considered**:
- Cypress e2e tests: out of scope for this feature's task set; would require backend running. Rejected.
- Snapshot tests: Angular CLI does not ship snapshot testing by default. Rejected.

---

## 5. Removing Angular Default Template

**Decision**: Replace `app.component.html` with a minimal shell containing only `<router-outlet />` and a top-level Bootstrap navbar for navigation.

**Rationale**: The Angular default template contains a large inline `<style>` block and extensive placeholder HTML. The entire file should be replaced with a minimal Bootstrap nav + router outlet to comply with FR-001 and to not conflict with Bootstrap's global reset styles.

**Alternatives considered**:
- Remove just the `<style>` block and SVG content, keep some structure: rejected — the existing template CSS variables conflict with Bootstrap's utility classes on some properties.
