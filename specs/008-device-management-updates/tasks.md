# Tasks: Device Management Updates

**Input**: Design documents from `/specs/008-device-management-updates/`

**Prerequisites**: plan.md (required), spec.md (required for user stories), research.md, data-model.md, contracts/

**Tests**: Included because the feature spec explicitly requires contract, integration, and frontend verification.

**Organization**: Tasks are grouped by user story so each story can be implemented and tested independently.

## Constitution-Driven Task Requirements

- Deliver an MVP vertical slice early: devices list with latest telemetry and a reachable details/removal path.
- For endpoint and payload changes, include contract, validation, and error-handling tasks.
- For telemetry/data changes, include persistence/projection and validation tasks.
- Include observability tasks for backend and frontend failure paths.
- Include security/configuration tasks for explicit CORS/origin rules and safe defaults.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (e.g., US1, US2, US3)
- Include exact file paths in descriptions

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Shared cross-cutting foundations used by all stories

- [X] T001 [P] Add Serilog package references and startup logging configuration in [backend/src/SmartHome.Api/Program.cs](backend/src/SmartHome.Api/Program.cs) and [backend/src/SmartHome.Api/appsettings.Development.json](backend/src/SmartHome.Api/appsettings.Development.json)
- [X] T002 [P] Add a context-safe fetch wrapper and shared request-failure logging entry points in [frontend/src/app/features/devices/services/devices-api.service.ts](frontend/src/app/features/devices/services/devices-api.service.ts) and [frontend/src/app/features/devices/services/device-pages.facade.ts](frontend/src/app/features/devices/services/device-pages.facade.ts)

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Core contract and data-access changes that every user story depends on

- [X] T003 [P] Extend device contracts to support unregister and complete registration payload fields in [backend/src/SmartHome.Api/api/contracts/DevicesContracts.cs](backend/src/SmartHome.Api/api/contracts/DevicesContracts.cs)
- [X] T004 [P] Add repository delete support and device lookup helpers for unregister flows in [backend/src/SmartHome.Api/infrastructure/repositories/DeviceRepository.cs](backend/src/SmartHome.Api/infrastructure/repositories/DeviceRepository.cs)
- [X] T005 [P] Add shared device view-model fields and routing placeholders for details navigation in [frontend/src/app/features/devices/models/device-pages.models.ts](frontend/src/app/features/devices/models/device-pages.models.ts) and [frontend/src/app/features/devices/devices.routes.ts](frontend/src/app/features/devices/devices.routes.ts)

**Checkpoint**: Foundation ready - user story work can now begin

---

## Phase 3: User Story 1 - Rich Device List (Priority: P1) 🎯 MVP

**Goal**: Display every device with its summary data, latest telemetry value, and expandable metadata on the devices page.

**Independent Test**: Open the devices page, confirm each row shows the latest telemetry value, expand a row, and verify all metadata is visible without navigating away.

### Tests for User Story 1

- [X] T006 [P] [US1] Add backend contract coverage for `GET /api/devices` response shape and telemetry summary fields in [backend/tests/SmartHome.Api.IntegrationTests/Contracts/DevicesListContractTests.cs](backend/tests/SmartHome.Api.IntegrationTests/Contracts/DevicesListContractTests.cs)
- [X] T007 [P] [US1] Extend frontend list component tests for expandable rows and visible latest telemetry rendering in [frontend/src/app/features/devices/device-list/device-list.component.spec.ts](frontend/src/app/features/devices/device-list/device-list.component.spec.ts)

### Implementation for User Story 1

- [X] T008 [US1] Update backend latest-telemetry projection to surface all summary fields in [backend/src/SmartHome.Api/services/LatestTelemetryQueryService.cs](backend/src/SmartHome.Api/services/LatestTelemetryQueryService.cs) and [backend/src/SmartHome.Api/api/endpoints/DevicesEndpoints.cs](backend/src/SmartHome.Api/api/endpoints/DevicesEndpoints.cs)
- [X] T009 [US1] Update frontend device list mapping to consume `latestMetricValue` and `latestEventTimeUtc` in [frontend/src/app/features/devices/services/device-pages.facade.ts](frontend/src/app/features/devices/services/device-pages.facade.ts)
- [X] T010 [US1] Implement expandable row UI and latest telemetry display in [frontend/src/app/features/devices/device-list/device-list.component.ts](frontend/src/app/features/devices/device-list/device-list.component.ts), [frontend/src/app/features/devices/device-list/device-list.component.html](frontend/src/app/features/devices/device-list/device-list.component.html), and [frontend/src/app/features/devices/device-list/device-list.component.css](frontend/src/app/features/devices/device-list/device-list.component.css)

**Checkpoint**: User Story 1 should be fully functional and independently testable

---

## Phase 4: User Story 2 - Device Details and Removal (Priority: P1)

**Goal**: Open a device details page from the list, show device data and telemetry, and allow unregistering the device from that page.

**Independent Test**: Click a device name to open the details page, confirm the device profile and telemetry are visible, and unregister the device successfully.

### Tests for User Story 2

- [X] T011 [P] [US2] Add backend integration tests for `DELETE /api/devices/{deviceId}` success and not-found cases in [backend/tests/SmartHome.Api.IntegrationTests/Devices/DeviceUnregisterTests.cs](backend/tests/SmartHome.Api.IntegrationTests/Devices/DeviceUnregisterTests.cs)
- [X] T012 [P] [US2] Add frontend details component tests for navigation, telemetry display, and unregister action state in [frontend/src/app/features/devices/device-details/device-details.component.spec.ts](frontend/src/app/features/devices/device-details/device-details.component.spec.ts)

### Implementation for User Story 2

- [X] T013 [US2] Implement backend unregister endpoint, validation, and OpenAPI metadata in [backend/src/SmartHome.Api/api/endpoints/DevicesEndpoints.cs](backend/src/SmartHome.Api/api/endpoints/DevicesEndpoints.cs) and [backend/src/SmartHome.Api/api/contracts/DevicesContracts.cs](backend/src/SmartHome.Api/api/contracts/DevicesContracts.cs)
- [X] T014 [US2] Add unregister support to the frontend API and details flow in [frontend/src/app/features/devices/services/devices-api.service.ts](frontend/src/app/features/devices/services/devices-api.service.ts), [frontend/src/app/features/devices/services/device-pages.facade.ts](frontend/src/app/features/devices/services/device-pages.facade.ts), [frontend/src/app/features/devices/device-details/device-details.component.ts](frontend/src/app/features/devices/device-details/device-details.component.ts), and [frontend/src/app/features/devices/device-details/device-details.component.html](frontend/src/app/features/devices/device-details/device-details.component.html)
- [X] T015 [US2] Wire device-name navigation from the list page to the details route in [frontend/src/app/features/devices/device-list/device-list.component.html](frontend/src/app/features/devices/device-list/device-list.component.html) and [frontend/src/app/features/devices/devices.routes.ts](frontend/src/app/features/devices/devices.routes.ts)

**Checkpoint**: User Story 2 should work independently from the list and removal flows

---

## Phase 5: User Story 3 - Complete Device Registration (Priority: P2)

**Goal**: Register a device with all fields required by the contract so the backend stores a complete device record.

**Independent Test**: Submit a complete Add Device form and verify the stored device record includes every required contract field; missing fields should surface a clear validation error.

### Tests for User Story 3

- [X] T016 [P] [US3] Add backend integration tests for `POST /api/devices` validation, duplicate external ID, and success paths in [backend/tests/SmartHome.Api.IntegrationTests/Devices/DeviceRegistrationTests.cs](backend/tests/SmartHome.Api.IntegrationTests/Devices/DeviceRegistrationTests.cs)
- [X] T017 [P] [US3] Extend frontend registration tests for complete contract submission and error handling in [frontend/src/app/features/devices/device-registration/device-registration.component.spec.ts](frontend/src/app/features/devices/device-registration/device-registration.component.spec.ts)

### Implementation for User Story 3

- [X] T018 [US3] Expand backend registration contract and persistence mapping in [backend/src/SmartHome.Api/api/contracts/DevicesContracts.cs](backend/src/SmartHome.Api/api/contracts/DevicesContracts.cs) and [backend/src/SmartHome.Api/api/endpoints/DevicesEndpoints.cs](backend/src/SmartHome.Api/api/endpoints/DevicesEndpoints.cs)
- [X] T019 [US3] Complete the Add Device form to capture all required contract fields in [frontend/src/app/features/devices/device-registration/device-registration.component.ts](frontend/src/app/features/devices/device-registration/device-registration.component.ts) and [frontend/src/app/features/devices/device-registration/device-registration.component.html](frontend/src/app/features/devices/device-registration/device-registration.component.html)
- [X] T020 [US3] Update frontend registration API submission and failure logging in [frontend/src/app/features/devices/services/devices-api.service.ts](frontend/src/app/features/devices/services/devices-api.service.ts) and [frontend/src/app/features/devices/device-registration/device-registration.component.ts](frontend/src/app/features/devices/device-registration/device-registration.component.ts)

**Checkpoint**: User Story 3 should be independently testable and persist complete device records

---

## Phase 6: User Story 4 - Telemetry Visibility and Delivery (Priority: P2)

**Goal**: Surface telemetry data so the latest value is visible in the list and details experiences after telemetry is ingested.

**Independent Test**: Ingest telemetry for a device, reload the UI, and confirm the latest telemetry value updates in the device summary and details views.

### Tests for User Story 4

- [X] T021 [P] [US4] Add backend integration tests for telemetry ingestion updating the latest visible value in [backend/tests/SmartHome.Api.IntegrationTests/Telemetry/TelemetryProjectionTests.cs](backend/tests/SmartHome.Api.IntegrationTests/Telemetry/TelemetryProjectionTests.cs)
- [X] T022 [P] [US4] Extend frontend facade tests for telemetry projection, fallback values, and safe fetch behavior in [frontend/src/app/features/devices/services/device-pages.facade.spec.ts](frontend/src/app/features/devices/services/device-pages.facade.spec.ts)

### Implementation for User Story 4

- [X] T023 [US4] Update telemetry ingestion and latest-projection logging in [backend/src/SmartHome.Api/services/TelemetryIngestionService.cs](backend/src/SmartHome.Api/services/TelemetryIngestionService.cs), [backend/src/SmartHome.Api/services/LatestTelemetryQueryService.cs](backend/src/SmartHome.Api/services/LatestTelemetryQueryService.cs), and [backend/src/SmartHome.Api/Program.cs](backend/src/SmartHome.Api/Program.cs)
- [X] T024 [US4] Surface telemetry history and details-page telemetry data in [frontend/src/app/features/devices/services/device-pages.facade.ts](frontend/src/app/features/devices/services/device-pages.facade.ts) and [frontend/src/app/features/devices/device-details/device-details.component.ts](frontend/src/app/features/devices/device-details/device-details.component.ts)
- [X] T025 [US4] Ensure list and details failures log the operation name and API base URL in [frontend/src/app/features/devices/services/device-pages.facade.ts](frontend/src/app/features/devices/services/device-pages.facade.ts) and [frontend/src/app/features/devices/device-list/device-list.component.ts](frontend/src/app/features/devices/device-list/device-list.component.ts)

**Checkpoint**: Telemetry should be visible in the summary experience and remain stable under reload-based updates

---

## Phase 7: Polish & Cross-Cutting Concerns

**Purpose**: Improvements that affect multiple user stories

- [X] T026 [P] Update [README.md](README.md) and [simulator/README.md](simulator/README.md) with device-management workflow, telemetry, and logging notes
- [X] T027 [P] Run [specs/008-device-management-updates/quickstart.md](specs/008-device-management-updates/quickstart.md) validation and capture any residual gaps in the feature docs

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies - can start immediately
- **Foundational (Phase 2)**: Depends on Setup completion - blocks all user stories
- **User Stories (Phase 3+)**: Depend on Foundational completion
- **Polish (Final Phase)**: Depends on the desired user stories being complete

### User Story Dependencies

- **User Story 1 (P1)**: Can start after Foundational; delivers the MVP list experience
- **User Story 2 (P1)**: Can start after Foundational; integrates with list navigation but remains independently testable
- **User Story 3 (P2)**: Can start after Foundational; independent registration flow
- **User Story 4 (P2)**: Can start after Foundational; independent telemetry visibility flow

### Within Each User Story

- Tests are written before implementation when practical and should fail before the fix lands.
- Backend contracts and repositories before endpoint wiring.
- Backend/API work before frontend integration.
- Core flow before polish and documentation updates.

### Parallel Opportunities

- Setup tasks T001 and T002 can run in parallel.
- Foundational tasks T003, T004, and T005 can run in parallel.
- User-story test tasks that touch different files can run in parallel.
- User stories can proceed in parallel after foundational work if team capacity allows.

---

## Parallel Example: User Story 1

```bash
Task: "Add backend contract coverage for GET /api/devices response shape and telemetry summary fields in backend/tests/SmartHome.Api.IntegrationTests/Contracts/DevicesListContractTests.cs"
Task: "Extend frontend list component tests for expandable rows and visible latest telemetry rendering in frontend/src/app/features/devices/device-list/device-list.component.spec.ts"
```

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1: Setup
2. Complete Phase 2: Foundational
3. Complete Phase 3: User Story 1
4. Validate the list page independently before moving on

### Incremental Delivery

1. Complete Setup + Foundational
2. Deliver User Story 1 and validate the list page
3. Add User Story 2 and validate device details/unregister
4. Add User Story 3 and validate complete registration
5. Add User Story 4 and validate telemetry visibility
6. Finish with polish and docs updates

### Parallel Team Strategy

With multiple developers:

1. Team completes Setup + Foundational together
2. Once Foundational is done:
   - Developer A: User Story 1
   - Developer B: User Story 2
   - Developer C: User Story 3
   - Developer D: User Story 4
3. Finish with documentation and quickstart validation

---

## Notes

- [P] tasks = different files, no dependencies
- [Story] label maps each task to a specific user story for traceability
- Keep backend contract fields as the source of truth when frontend and simulator code diverge
- Preserve explicit logging and error-handling behavior while implementing the flows