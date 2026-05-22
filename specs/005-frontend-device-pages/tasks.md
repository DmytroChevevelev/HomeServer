# Tasks: Frontend Device Pages

**Input**: Design documents from `/specs/005-frontend-device-pages/`

**Prerequisites**: plan.md (required), spec.md (required for user stories), research.md, data-model.md, contracts/

**Tests**: Tests are included because the specification explicitly requires minimum automated coverage for route navigation, list rendering states, date-time filtering behavior, and add/register navigation.

**Organization**: Tasks are grouped by user story to enable independent implementation and testing of each story.

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Align routing and shared frontend scaffolding for the feature.

- [X] T001 Add frontend device-pages feature README section and route overview in frontend/README.md
- [X] T002 Add base devices feature routing shell for list/details/register pages in frontend/src/app/features/devices/devices.routes.ts
- [X] T003 [P] Add shared device view-model type definitions in frontend/src/app/features/devices/models/device-pages.models.ts
- [X] T004 [P] Add date-time filter utility helpers for validation and formatting in frontend/src/app/features/devices/utils/date-time-filter.util.ts

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Build shared services/state utilities required by all user stories.

**⚠️ CRITICAL**: No user story work can begin until this phase is complete.

- [X] T005 Extend root app routing to mount devices feature routes in frontend/src/app/app.routes.ts
- [X] T006 Implement devices data facade for list/detail/history retrieval in frontend/src/app/features/devices/services/device-pages.facade.ts
- [X] T007 Implement UI state mapping helper for loading/empty/error states in frontend/src/app/features/devices/services/device-pages-state.mapper.ts
- [X] T008 [P] Add foundational service tests for facade contract mapping in frontend/src/app/features/devices/services/device-pages.facade.spec.ts
- [X] T009 [P] Add foundational tests for date-time filter validation utility in frontend/src/app/features/devices/utils/date-time-filter.util.spec.ts

**Checkpoint**: Foundation ready - user story implementation can begin.

---

## Phase 3: User Story 1 - View Device List Dashboard (Priority: P1) 🎯 MVP

**Goal**: Show device list on main page with status indicator, type, and value/no-value state.

**Independent Test**: Load main page with seeded devices and verify row status indicator, type, and value-or-placeholder are rendered.

### Tests for User Story 1

- [ ] T010 [P] [US1] Add component test for populated list rendering states in frontend/src/app/features/devices/device-list/device-list.component.spec.ts
- [ ] T011 [P] [US1] Add component test for empty and error list states in frontend/src/app/features/devices/device-list/device-list-empty-error.spec.ts

### Implementation for User Story 1

- [ ] T012 [US1] Implement device list page view-model mapping and rendering in frontend/src/app/features/devices/device-list/device-list.component.ts
- [ ] T013 [US1] Implement device list template with status color indicator and value placeholder states in frontend/src/app/features/devices/device-list/device-list.component.html
- [ ] T014 [US1] Add device list styling for status indicator tokens and row layout in frontend/src/app/features/devices/device-list/device-list.component.css
- [ ] T015 [US1] Wire main page route to device list page in frontend/src/app/features/devices/devices.routes.ts

**Checkpoint**: User Story 1 is independently functional and testable.

---

## Phase 4: User Story 2 - Navigate Device Details (Priority: P2)

**Goal**: Device row navigation opens details page with header, history list, and date-time filtering.

**Independent Test**: Click a device row from list and verify details route, header context, history list, and filter apply behavior.

### Tests for User Story 2

- [ ] T016 [P] [US2] Add route-navigation test from list row to details page in frontend/src/app/features/devices/device-list/device-list-navigation.spec.ts
- [ ] T017 [P] [US2] Add component test for details header and history rendering in frontend/src/app/features/devices/device-details/device-details.component.spec.ts
- [ ] T018 [P] [US2] Add component test for valid and invalid date-time filter interactions in frontend/src/app/features/devices/device-details/device-history-filter.spec.ts

### Implementation for User Story 2

- [ ] T019 [US2] Implement clickable row navigation action from list to details in frontend/src/app/features/devices/device-list/device-list.component.ts
- [ ] T020 [US2] Create device details component logic for header/history/filter state in frontend/src/app/features/devices/device-details/device-details.component.ts
- [ ] T021 [US2] Create device details template with header section and historical list in frontend/src/app/features/devices/device-details/device-details.component.html
- [ ] T022 [US2] Add date-time filter form controls and invalid-range feedback handling in frontend/src/app/features/devices/device-details/device-details.component.ts
- [ ] T023 [US2] Register device details route in devices feature routes in frontend/src/app/features/devices/devices.routes.ts

**Checkpoint**: User Stories 1 and 2 are independently functional and testable.

---

## Phase 5: User Story 3 - Start Device Registration Flow (Priority: P3)

**Goal**: Main page add/register button navigates to registration page.

**Independent Test**: From main list page, use add/register action and verify registration route loads.

### Tests for User Story 3

- [ ] T024 [P] [US3] Add component test for add/register button visibility and action in frontend/src/app/features/devices/device-list/device-list-register-action.spec.ts
- [ ] T025 [P] [US3] Add route test for registration-page navigation from main page action in frontend/src/app/features/devices/device-registration/device-registration-navigation.spec.ts

### Implementation for User Story 3

- [ ] T026 [US3] Add add/register button and click handler to main list page in frontend/src/app/features/devices/device-list/device-list.component.html
- [ ] T027 [US3] Wire add/register action navigation in list component logic in frontend/src/app/features/devices/device-list/device-list.component.ts
- [ ] T028 [US3] Register device registration route and component mapping in frontend/src/app/features/devices/devices.routes.ts

**Checkpoint**: All user stories are independently functional and testable.

---

## Phase 6: Polish & Cross-Cutting Concerns

**Purpose**: Finalize contract/docs/tests and quality checks across all stories.

- [ ] T029 [P] Align UI contract details with implemented routes and filter behavior in specs/005-frontend-device-pages/contracts/frontend-device-pages.yaml
- [ ] T030 [P] Update quickstart with final route names, filter behavior, and validation outcomes in specs/005-frontend-device-pages/quickstart.md
- [ ] T031 Run frontend unit tests and record results in specs/005-frontend-device-pages/quickstart.md
- [ ] T032 Run frontend build verification and record result in specs/005-frontend-device-pages/quickstart.md
- [ ] T033 Update frontend TODO section with delivered feature status in frontend/README.md

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies - can start immediately.
- **Foundational (Phase 2)**: Depends on Setup completion - blocks all user stories.
- **User Story Phases (3-5)**: Depend on Foundational completion.
- **Polish (Phase 6)**: Depends on all user stories being complete.

### User Story Dependencies

- **User Story 1 (P1)**: Starts after Phase 2; no dependency on other stories.
- **User Story 2 (P2)**: Starts after Phase 2 and integrates list navigation from US1.
- **User Story 3 (P3)**: Starts after Phase 2 and can be implemented alongside US2 once list actions exist.

### Within Each User Story

- Tests should be added first and fail before implementation completion.
- Component logic before template polish.
- Routing updates before final navigation validation.

### Parallel Opportunities

- Phase 1 tasks T003 and T004 are parallel.
- Phase 2 tests T008 and T009 are parallel.
- US1 tests T010 and T011 are parallel.
- US2 tests T016, T017, and T018 are parallel.
- US3 tests T024 and T025 are parallel.
- Polish documentation tasks T029 and T030 are parallel.

---

## Parallel Example: User Story 2

```bash
# Run US2 tests in parallel:
Task: "Add route-navigation test from list row to details page in frontend/src/app/features/devices/device-list/device-list-navigation.spec.ts"
Task: "Add component test for details header and history rendering in frontend/src/app/features/devices/device-details/device-details.component.spec.ts"
Task: "Add component test for valid and invalid date-time filter interactions in frontend/src/app/features/devices/device-details/device-history-filter.spec.ts"
```

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1 setup tasks.
2. Complete Phase 2 foundational tasks.
3. Complete Phase 3 (US1) device list dashboard.
4. Validate list status/value/type behavior before moving forward.

### Incremental Delivery

1. Deliver US1 list dashboard with clear states.
2. Deliver US2 details navigation and history filtering.
3. Deliver US3 add/register navigation action.
4. Finish with Phase 6 contract/doc/test verification.

### Parallel Team Strategy

1. Developer A: list/dashboard components and states (US1).
2. Developer B: details page and filter logic (US2).
3. Developer C: registration navigation and docs/tests updates (US3 + polish).

---

## Notes

- [P] tasks are safe to parallelize across different files.
- [USx] labels map tasks to independently testable user stories.
- Each task includes a concrete file path for direct execution.
