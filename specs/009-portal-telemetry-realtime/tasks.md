# Tasks: Portal Telemetry Realtime

**Input**: Design documents from `/specs/009-portal-telemetry-realtime/`

**Prerequisites**: plan.md (required), spec.md (required for user stories), research.md, data-model.md, contracts/

**Tests**: Automated tests are included because the specification explicitly requires minimum backend integration and frontend unit/component coverage.

**Organization**: Tasks are grouped by user story to enable independent implementation and testing of each story.

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Prepare dependencies and shared contracts for real-time telemetry and selected-device history flows.

- [X] T001 Add SignalR server dependency and XML documentation settings in backend/src/SmartHome.Api/SmartHome.Api.csproj
- [X] T002 [P] Add SignalR client dependency for Angular in frontend/package.json
- [X] T003 Create telemetry realtime and history DTO contracts in backend/src/SmartHome.Api/api/contracts/TelemetryContracts.cs
- [X] T004 [P] Add frontend DTO types for telemetry history and realtime payloads in frontend/src/app/features/devices/models/device-pages.models.ts

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Establish backend notification plumbing and frontend realtime service scaffolding required by all user stories.

**⚠️ CRITICAL**: No user story work starts until this phase is complete.

- [X] T005 Register SignalR services and map telemetry hub route in backend/src/SmartHome.Api/Program.cs
- [X] T006 Create telemetry notification hub in backend/src/SmartHome.Api/api/hubs/TelemetryHub.cs
- [X] T007 [P] Add telemetry history repository query methods in backend/src/SmartHome.Api/infrastructure/repositories/TelemetryRepository.cs
- [X] T008 [P] Add selected-device telemetry query service in backend/src/SmartHome.Api/services/DeviceTelemetryHistoryQueryService.cs
- [X] T009 Register DeviceTelemetryHistoryQueryService in backend/src/SmartHome.Api/Program.cs
- [X] T010 Create frontend SignalR connection wrapper service in frontend/src/app/features/devices/services/device-telemetry-realtime.service.ts
- [X] T011 [P] Add telemetry realtime subscription tests scaffold in frontend/src/app/features/devices/services/device-telemetry-realtime.service.spec.ts

**Checkpoint**: Foundation complete; user stories can be implemented independently.

---

## Phase 3: User Story 1 - Real-Time Sensor Monitoring (Priority: P1) 🎯 MVP

**Goal**: Device list latest values update automatically when telemetry changes are ingested.

**Independent Test**: Ingest new telemetry for a listed device and observe Latest Value update in-session without page reload.

### Tests for User Story 1

- [ ] T012 [P] [US1] Add backend integration test for telemetry ingest notification publish in backend/tests/SmartHome.Api.IntegrationTests/Telemetry/TelemetryRealtimeNotificationsTests.cs
- [ ] T013 [P] [US1] Add frontend device-list realtime update test in frontend/src/app/features/devices/device-list/device-list.component.spec.ts

### Implementation for User Story 1

- [X] T014 [US1] Publish sensorValueChanged SignalR event after successful ingest in backend/src/SmartHome.Api/api/endpoints/TelemetryEndpoints.cs
- [X] T015 [US1] Add notification publish logging and failure handling in backend/src/SmartHome.Api/api/endpoints/TelemetryEndpoints.cs
- [X] T016 [US1] Wire realtime subscription lifecycle into device list component in frontend/src/app/features/devices/device-list/device-list.component.ts
- [X] T017 [US1] Apply realtime latest-value patch logic in frontend/src/app/features/devices/services/device-pages.facade.ts
- [X] T018 [US1] Render realtime connection/error feedback in device list view in frontend/src/app/features/devices/device-list/device-list.component.html

**Checkpoint**: User Story 1 is independently functional and testable.

---

## Phase 4: User Story 2 - Selected Device Telemetry History (Priority: P1)

**Goal**: Device details page displays selected-device telemetry rows from a dedicated backend endpoint.

**Independent Test**: Open a selected device and verify telemetry rows load in newest-first order, with clear empty state when no data exists.

### Tests for User Story 2

- [ ] T019 [P] [US2] Add backend integration test for GET /api/devices/{deviceId}/telemetry ordering and filtering in backend/tests/SmartHome.Api.IntegrationTests/Devices/DeviceTelemetryHistoryEndpointTests.cs
- [ ] T020 [P] [US2] Add frontend device-details telemetry list rendering test in frontend/src/app/features/devices/device-details/device-details.component.spec.ts
- [ ] T021 [P] [US2] Add facade contract-mapping test for selected-device telemetry list in frontend/src/app/features/devices/services/device-pages.facade.spec.ts

### Implementation for User Story 2

- [ ] T022 [US2] Implement GET /api/devices/{deviceId}/telemetry endpoint with OpenAPI metadata in backend/src/SmartHome.Api/api/endpoints/DevicesEndpoints.cs
- [ ] T023 [US2] Add telemetry history query and validation logic in backend/src/SmartHome.Api/services/DeviceTelemetryHistoryQueryService.cs
- [ ] T024 [US2] Add endpoint parameter and response contracts in backend/src/SmartHome.Api/api/contracts/DevicesContracts.cs
- [ ] T025 [US2] Update runtime OpenAPI summary entries for new telemetry list endpoint in backend/src/SmartHome.Api/api/contracts/openapi.yaml
- [ ] T026 [US2] Replace history loading call to selected-device endpoint in frontend/src/app/features/devices/services/device-pages.facade.ts
- [ ] T027 [US2] Replace Telemetry section with telemetry list presentation in frontend/src/app/features/devices/device-details/device-details.component.html
- [ ] T028 [US2] Update device-details telemetry state and mapping flow in frontend/src/app/features/devices/device-details/device-details.component.ts

**Checkpoint**: User Stories 1 and 2 both work independently.

---

## Phase 5: User Story 3 - Reliable Monitoring Feedback (Priority: P2)

**Goal**: Monitoring remains understandable during transient failures and recovers automatically when services return.

**Independent Test**: Simulate endpoint and realtime interruptions; verify visible feedback appears and updates resume after recovery.

### Tests for User Story 3

- [ ] T029 [P] [US3] Add frontend realtime disconnect/reconnect UI-state test in frontend/src/app/features/devices/device-list/device-list.component.spec.ts
- [ ] T030 [P] [US3] Add backend integration test for telemetry list endpoint 400/404 responses in backend/tests/SmartHome.Api.IntegrationTests/Devices/DeviceTelemetryHistoryEndpointTests.cs

### Implementation for User Story 3

- [ ] T031 [US3] Add date-range and limit validation error handling for telemetry list endpoint in backend/src/SmartHome.Api/api/endpoints/DevicesEndpoints.cs
- [ ] T032 [US3] Add reconnect and error-state callbacks in frontend/src/app/features/devices/services/device-telemetry-realtime.service.ts
- [ ] T033 [US3] Add resilient fallback messaging for telemetry history load failures in frontend/src/app/features/devices/device-details/device-details.component.html
- [ ] T034 [US3] Add operational diagnostics for selected-device telemetry retrieval failures in frontend/src/app/features/devices/services/device-pages.facade.ts

**Checkpoint**: All user stories are independently functional.

---

## Phase 6: Polish & Cross-Cutting Concerns

**Purpose**: Finalize documentation, regression confidence, and end-to-end validation.

- [ ] T035 [P] Update API and realtime workflow documentation in backend/README.md
- [ ] T036 [P] Update portal device monitoring behavior notes in frontend/README.md
- [ ] T037 [P] Add developer notes for telemetry endpoint usage and verification in docs/development-commands.md
- [ ] T038 Run end-to-end feature validation scenarios from specs/009-portal-telemetry-realtime/quickstart.md

---

## Dependencies & Execution Order

### Phase Dependencies

- **Phase 1 (Setup)**: No dependencies.
- **Phase 2 (Foundational)**: Depends on Phase 1 and blocks all user story work.
- **Phase 3 (US1)**: Depends on Phase 2; delivers MVP real-time list updates.
- **Phase 4 (US2)**: Depends on Phase 2; can run in parallel with US1 after foundation if staffed.
- **Phase 5 (US3)**: Depends on US1 and US2 implementation paths for realistic resilience checks.
- **Phase 6 (Polish)**: Depends on completion of all targeted user stories.

### User Story Dependencies

- **US1 (P1)**: No dependency on other stories after foundational work.
- **US2 (P1)**: No dependency on US1 business logic, but shares foundational realtime/backend plumbing.
- **US3 (P2)**: Depends on US1 realtime flow and US2 endpoint/details flow to validate failure and recovery behavior.

### Within Each User Story

- Tests should be added first and verified failing before implementation changes.
- Endpoint/service changes precede UI wiring for the same contract.
- UI behavior and diagnostics complete before story checkpoint sign-off.

### Parallel Opportunities

- T002, T004 can run in parallel in Setup.
- T007, T008, T011 can run in parallel in Foundational.
- US1 tests T012 and T013 can run in parallel.
- US2 tests T019, T020, T021 can run in parallel.
- Polish documentation tasks T035, T036, T037 can run in parallel.

---

## Parallel Example: User Story 2

```bash
# Run contract and UI tests for US2 in parallel:
Task: "T019 [US2] Add backend integration test for GET /api/devices/{deviceId}/telemetry ordering and filtering in backend/tests/SmartHome.Api.IntegrationTests/Devices/DeviceTelemetryHistoryEndpointTests.cs"
Task: "T020 [US2] Add frontend device-details telemetry list rendering test in frontend/src/app/features/devices/device-details/device-details.component.spec.ts"
Task: "T021 [US2] Add facade contract-mapping test for selected-device telemetry list in frontend/src/app/features/devices/services/device-pages.facade.spec.ts"

# Implement backend and frontend pieces in parallel once contracts are stable:
Task: "T023 [US2] Add telemetry history query and validation logic in backend/src/SmartHome.Api/services/DeviceTelemetryHistoryQueryService.cs"
Task: "T027 [US2] Replace Telemetry section with telemetry list presentation in frontend/src/app/features/devices/device-details/device-details.component.html"
```

---

## Implementation Strategy

### MVP First (US1)

1. Complete Phase 1 and Phase 2.
2. Complete Phase 3 (US1) and validate realtime list updates.
3. Demo MVP with simulator-driven telemetry ingest.

### Incremental Delivery

1. Deliver US1 realtime list behavior.
2. Deliver US2 selected-device telemetry list endpoint and details UI rendering.
3. Deliver US3 resilience and recovery feedback.
4. Finish with documentation and quickstart validation.

### Parallel Team Strategy

1. Team finishes Setup + Foundational together.
2. Developer A implements US1; Developer B implements US2.
3. Developer C verifies US3 resilience tasks after US1/US2 merge.
4. Team closes with Polish phase and quickstart verification.

---

## Notes

- [P] tasks target separate files and minimal dependency overlap.
- Every user-story task includes a [USx] label for traceability.
- Keep backend contracts as source of truth for frontend mapping (`latestMetricValue`, `latestEventTimeUtc`).
- Ensure all modified endpoints include OpenAPI summary, parameter descriptions, and response descriptions.
