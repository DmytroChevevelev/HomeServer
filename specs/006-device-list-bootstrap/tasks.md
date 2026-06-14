- [x] T017 [P] Update `README.md`
- [x] T018 Run `ng build`
- [x] T019 Run `ng test --watch=false --browsers=ChromeHeadless`
- [x] T016 [US2] Add a no-match empty-state message inside the `ready` block
# Tasks: Device List with Bootstrap and Status Filter

**Input**: Design documents from `specs/006-device-list-bootstrap/`

**Prerequisites**: [plan.md](plan.md) | [spec.md](spec.md) | [research.md](research.md) | [data-model.md](data-model.md) | [contracts/ui-contracts.md](contracts/ui-contracts.md)

## Phase 1: Setup (Bootstrap Integration)

**Purpose**: Install Bootstrap and remove Angular default template — prerequisite for all US1 UI work.

- [x] T001 Install `bootstrap` npm package in `frontend/` by running `npm install bootstrap` and verifying it appears in `frontend/package.json` dependencies
- [x] T002 Add `node_modules/bootstrap/dist/css/bootstrap.min.css` to the `styles` array in `frontend/angular.json` under `projects.smart-home-portal-frontend.architect.build.options.styles`, before `src/styles.css`
- [x] T003 Replace entire contents of `frontend/src/app/app.component.html` with a minimal Bootstrap navbar shell and `<router-outlet />` per [contracts/ui-contracts.md](contracts/ui-contracts.md) Shell Component Contract section
- [x] T004 Clear `frontend/src/app/app.component.css` of all Angular placeholder styles (leave file empty or with a single comment)

**Checkpoint**: `ng build` passes; `http://localhost:4200` shows Bootstrap navbar with no Angular placeholder content.

---

## Phase 2: Foundational (Model and Facade Fix)

**Purpose**: Core changes that US1 and US2 both depend on — data model extension and the `normalizeStatus()` bug fix.

**⚠️ CRITICAL**: US1 device list rendering depends on the facade fix; `"active"` / `"stale"` currently map to `"unknown"` causing all status badges to render wrong.

- [x] T005 Add `export type StatusFilterOption = DeviceStatus | 'all'` to `frontend/src/app/features/devices/models/device-pages.models.ts` (append after the `UiSurfaceState` interface)
- [x] T006 Fix `normalizeStatus()` in `frontend/src/app/features/devices/services/device-pages.facade.ts`: add `case 'active': return 'online'` and `case 'stale': return 'offline'` to the switch statement before the `default` case

**Checkpoint**: Facade `normalizeStatus()` unit tests for `'active'` → `'online'` and `'stale'` → `'offline'` pass.

---

## Phase 3: User Story 1 — Bootstrap Device List (Priority: P1) 🎯 MVP

**Goal**: Operator opens `/devices` and sees all registered devices in a Bootstrap table with color-coded status badges; loading, empty, and error states all render using Bootstrap components.

**Independent Test**: Open `http://localhost:4200/devices` with backend running — Bootstrap table renders device rows with status badges; Angular placeholder is gone.

### Tests for User Story 1

- [x] T007 [P] [US1] Add 2 tests to `frontend/src/app/features/devices/services/device-pages.facade.spec.ts`: verify that a mocked API response with `status: 'active'` produces `DeviceStatus 'online'`, and `status: 'stale'` produces `DeviceStatus 'offline'`
- [x] T008 [P] [US1] Create `frontend/src/app/features/devices/device-list/device-list.component.spec.ts` with 4 tests: (1) loading state renders a `[class*="spinner"]` element, (2) error state renders `[class*="alert-danger"]`, (3) empty state renders `[class*="alert-info"]`, (4) ready state with 3 mocked `DeviceListItemViewModel` items renders exactly 3 `<tr>` rows inside `<tbody>`

### Implementation for User Story 1

- [x] T009 [US1] Implement `DeviceListComponent` class in `frontend/src/app/features/devices/device-list/device-list.component.ts`: inject `DevicePagesFacade`, add `allDevices: DeviceListItemViewModel[]`, `uiState: UiSurfaceState`, `selectedStatus: StatusFilterOption = 'all'`; implement `ngOnInit()` calling `facade.getDeviceList()`; implement `get filteredDevices()` getter filtering `allDevices` by `selectedStatus`; implement `badgeClass(status: DeviceStatus): string` returning Bootstrap badge class per [data-model.md](data-model.md) Bootstrap Badge Class Mapping table; add `OnInit` to imports
- [x] T010 [US1] Implement `frontend/src/app/features/devices/device-list/device-list.component.html`: Bootstrap spinner for `loading` state, `alert-danger` for `error` state (showing `uiState.errorMessage`), `alert-info` for `empty` state, and `table table-hover` for `ready` state with `*ngFor` over `filteredDevices` — columns: Name, External ID, Type, Status badge using `[ngClass]="badgeClass(device.status)"`
- [x] T011 [US1] Update `device-list.component.ts` imports array to include `NgIf`, `NgFor`, `NgClass` from `@angular/common` (or keep `CommonModule`) and ensure `DevicePagesFacade` is resolvable
- [x] T012 [US1] Clear `frontend/src/app/features/devices/device-list/device-list.component.css` of placeholder styles; add only minimal container padding if needed

**Checkpoint**: US1 complete — Bootstrap table renders live device data; all 4 component tests + 2 facade tests pass.

---

## Phase 4: User Story 2 — Status Filter (Priority: P2)

**Goal**: Operator selects a status value from a Bootstrap filter control and the device list immediately updates to show only matching devices; "All" resets to full list.

**Independent Test**: With at least two devices of different statuses, select "Offline" in the filter — list narrows to offline devices only; select "All" — full list returns.

### Tests for User Story 2

- [x] T013 [P] [US2] Add 2 tests to `frontend/src/app/features/devices/device-list/device-list.component.spec.ts`: (1) `selectedStatus = 'offline'` with mixed-status items renders only offline rows, (2) resetting `selectedStatus = 'all'` renders all rows again

### Implementation for User Story 2

- [x] T014 [US2] Add `setFilter(status: StatusFilterOption): void` method to `DeviceListComponent` in `frontend/src/app/features/devices/device-list/device-list.component.ts` that sets `this.selectedStatus = status`
- [x] T015 [US2] Add Bootstrap filter control to `frontend/src/app/features/devices/device-list/device-list.component.html` above the table: a `<select class="form-select form-select-sm w-auto">` with `(change)` bound to `setFilter($event.target.value)` and `<option>` values for `all`, `online`, `offline`, `unknown` (labels: All, Online, Offline, Unknown); show the filter only when `uiState.status === 'ready'`
- [ ] T016 [US2] Add a no-match empty-state message inside the `ready` block in `frontend/src/app/features/devices/device-list/device-list.component.html`: when `filteredDevices.length === 0` and `selectedStatus !== 'all'`, show `<div class="alert alert-warning">` with message "No devices match the selected status."

**Checkpoint**: US2 complete — filter control updates list synchronously; no-match state shows Bootstrap warning alert; 2 new filter tests pass.

---

## Phase 5: Polish & Cross-Cutting Concerns

**Purpose**: Final cleanup, doc update, and build verification.

- [ ] T017 [P] Update `README.md` (frontend section) or `docs/development-commands.md` to document the Bootstrap install step and the status filter behavior for developers new to this feature
- [ ] T018 Run `ng build` in `frontend/` and confirm clean build with no TypeScript errors and no missing Bootstrap class warnings
- [ ] T019 Run `ng test --watch=false --browsers=ChromeHeadless` for all device feature specs and confirm all tests pass (existing 7 + new 8 = 15 total)

---

## Dependencies & Execution Order

### Phase Dependencies

```
Phase 1 (Bootstrap Setup) ─────────────────────────────┐
                                                         ↓
Phase 2 (Model + Facade Fix) ──────────────────────────→ Phase 3 (US1) ──→ Phase 4 (US2) ──→ Phase 5 (Polish)
```

- **Phase 1**: No dependencies — install Bootstrap, swap template
- **Phase 2**: No dependencies — can run in parallel with Phase 1
- **Phase 3 (US1)**: Depends on both Phase 1 and Phase 2
- **Phase 4 (US2)**: Depends on Phase 3 (needs `selectedStatus` field and `filteredDevices` getter from T009)
- **Phase 5 (Polish)**: Depends on all phases complete

### Parallel Execution Within Phases

- **T007 and T008** can run in parallel (different files)
- **T013** can be written while T009–T011 are in progress (test file, separate from component implementation)
- **T017** can run at any time after Phase 1

### MVP Scope

**Minimum viable demo** = Phase 1 + Phase 2 + Phase 3 (T001–T012). Delivers: Bootstrap navbar, device table with status badges, loading/error/empty states. Filter (Phase 4) is the next slice.

---

## Summary

| Metric | Value |
|---|---|
| Total tasks | 19 |
| Phase 1 (Setup) | 4 tasks |
| Phase 2 (Foundational) | 2 tasks |
| Phase 3 (US1 — MVP) | 6 tasks |
| Phase 4 (US2 — Filter) | 4 tasks |
| Phase 5 (Polish) | 3 tasks |
| Parallelizable [P] tasks | T007, T008, T013, T017 |
| New unit tests | 8 (2 facade + 6 component) |
| Files changed | 9 |
| Backend changes | None |
