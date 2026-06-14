# Tasks: Portal Telemetry Realtime

**Input**: Design documents from `/specs/009-portal-telemetry-realtime/`

**Prerequisites**: plan.md (required), spec.md (required for user stories), research.md, data-model.md, contracts/

**Tests**: Include automated tests because the specification and plan explicitly require minimum backend integration and frontend unit/component confidence.

**Organization**: Tasks are grouped by user story for independent implementation and validation.

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Align dependencies, contracts, and baseline metadata before feature implementation.

- [X] T001 Add SignalR and XML docs configuration in backend/src/SmartHome.Api/SmartHome.Api.csproj
- [X] T002 [P] Add frontend SignalR client dependency in frontend/package.json
- [X] T003 Define telemetry history and realtime notification contracts in backend/src/SmartHome.Api/api/contracts/TelemetryContracts.cs
- [X] T004 [P] Add device telemetry/realtime contract types in frontend/src/app/features/devices/models/device-pages.models.ts

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Establish backend and frontend primitives required by all user stories.

**⚠️ CRITICAL**: User stories cannot proceed until this phase completes.

- [X] T005 Register SignalR and telemetry history query service in backend/src/SmartHome.Api/Program.cs
- [X] T006 Add telemetry SignalR hub definition in backend/src/SmartHome.Api/api/hubs/TelemetryHub.cs
- [X] T007 [P] Add telemetry repository list/latest helpers in backend/src/SmartHome.Api/infrastructure/repositories/TelemetryRepository.cs
- [X] T008 [P] Add selected-device telemetry query service in backend/src/SmartHome.Api/services/DeviceTelemetryHistoryQueryService.cs
- [X] T009 Add telemetry cleanup on unregister in backend/src/SmartHome.Api/infrastructure/repositories/DeviceRepository.cs
- [X] T010 Add frontend realtime connection service in frontend/src/app/features/devices/services/device-telemetry-realtime.service.ts
- [X] T011 [P] Add realtime service scaffold test in frontend/src/app/features/devices/services/device-telemetry-realtime.service.spec.ts

**Checkpoint**: Foundation complete; user stories can begin.

---

## Phase 3: User Story 1 - Real-Time Sensor Monitoring (Priority: P1) 🎯 MVP

**Goal**: Device list latest values refresh automatically when telemetry changes occur.

**Independent Test**: Ingest new telemetry and verify device list latest value updates without page reload.

### Tests for User Story 1

- [X] T012 [P] [US1] Add backend integration test for telemetry publish path in backend/tests/SmartHome.Api.IntegrationTests/Telemetry/TelemetryRealtimeNotificationsTests.cs
- [X] T013 [P] [US1] Add frontend realtime list update test in frontend/src/app/features/devices/device-list/device-list.component.spec.ts

### Implementation for User Story 1

- [X] T014 [US1] Publish sensorValueChanged after telemetry ingest in backend/src/SmartHome.Api/api/endpoints/TelemetryEndpoints.cs
- [X] T015 [US1] Add publish success/failure diagnostics in backend/src/SmartHome.Api/api/endpoints/TelemetryEndpoints.cs
- [X] T016 [US1] Wire device-list realtime subscribe/start/stop lifecycle in frontend/src/app/features/devices/device-list/device-list.component.ts
- [X] T017 [US1] Add reusable realtime patch mapper in frontend/src/app/features/devices/services/device-pages.facade.ts
- [X] T018 [US1] Show non-blocking realtime warning state in frontend/src/app/features/devices/device-list/device-list.component.html

**Checkpoint**: US1 independently functional.

---

## Phase 4: User Story 2 - Selected Device Telemetry History (Priority: P1)

**Goal**: Device details show selected-device telemetry list from dedicated endpoint with default 100 and user-controlled limit.

**Independent Test**: Open a selected device and verify newest-first list from `/api/devices/{deviceId}/telemetry`, default 100 rows, custom limit respected.

### Tests for User Story 2

- [X] T019 [P] [US2] Add backend integration test for default/custom limit behavior in backend/tests/SmartHome.Api.IntegrationTests/Devices/DeviceTelemetryHistoryEndpointTests.cs
- [X] T020 [P] [US2] Add backend integration test for selected-device filtering/order in backend/tests/SmartHome.Api.IntegrationTests/Devices/DeviceTelemetryHistoryEndpointTests.cs
- [X] T021 [P] [US2] Add frontend details telemetry rendering test in frontend/src/app/features/devices/device-details/device-details.component.spec.ts
- [X] T022 [P] [US2] Add frontend facade contract-mapping test for telemetry endpoint response in frontend/src/app/features/devices/services/device-pages.facade.spec.ts

### Implementation for User Story 2

- [X] T023 [US2] Add GET /api/devices/{deviceId}/telemetry endpoint with limit validation and OpenAPI metadata in backend/src/SmartHome.Api/api/endpoints/DevicesEndpoints.cs
- [X] T024 [US2] Add devices telemetry response contracts in backend/src/SmartHome.Api/api/contracts/DevicesContracts.cs
- [X] T025 [US2] Update runtime baseline route summary in backend/src/SmartHome.Api/api/contracts/openapi.yaml
- [X] T026 [US2] Switch details history API call to selected-device endpoint in frontend/src/app/features/devices/services/device-pages.facade.ts
- [X] T027 [US2] Add user-selectable telemetry limit state and reload flow in frontend/src/app/features/devices/device-details/device-details.component.ts
- [X] T028 [US2] Add telemetry limit input/actions to details UI in frontend/src/app/features/devices/device-details/device-details.component.html

**Checkpoint**: US2 independently functional.

---

## Phase 5: User Story 3 - Reliable Monitoring Feedback (Priority: P2)

**Goal**: Monitoring remains understandable during endpoint/realtime failures and cleanup operations remain safe.

**Independent Test**: Simulate temporary endpoint/realtime failures and validate clear UI feedback plus recovery behavior.

### Tests for User Story 3

- [X] T029 [P] [US3] Add frontend realtime disconnect/reconnect state test in frontend/src/app/features/devices/device-list/device-list.component.spec.ts
- [X] T030 [P] [US3] Add backend integration test for invalid telemetry limit returning 400 in backend/tests/SmartHome.Api.IntegrationTests/Devices/DeviceTelemetryHistoryEndpointTests.cs
- [X] T031 [P] [US3] Add backend integration test verifying unregister removes related telemetry in backend/tests/SmartHome.Api.IntegrationTests/Devices/DeviceUnregisterEndpointTests.cs

### Implementation for User Story 3

- [X] T032 [US3] Add explicit unregister telemetry cleanup logging in backend/src/SmartHome.Api/api/endpoints/DevicesEndpoints.cs
- [X] T033 [US3] Add resilient telemetry-history failure diagnostics in frontend/src/app/features/devices/services/device-pages.facade.ts
- [X] T034 [US3] Refine details-page failure/empty states for telemetry list in frontend/src/app/features/devices/device-details/device-details.component.html

**Checkpoint**: All stories independently functional.

---

## Phase 6: Polish & Cross-Cutting Concerns

**Purpose**: Cross-story hardening and documentation.

- [X] T035 [P] Update backend API docs for selected-device telemetry route and unregister cleanup in backend/README.md
- [X] T036 [P] Update frontend usage notes for telemetry limit and realtime warnings in frontend/README.md
- [X] T037 [P] Add developer run/verify commands for telemetry endpoint and realtime checks in docs/development-commands.md
- [X] T038 Execute quickstart validation scenarios in specs/009-portal-telemetry-realtime/quickstart.md

---

## Dependencies & Execution Order

### Phase Dependencies

- **Phase 1 (Setup)**: No dependencies.
- **Phase 2 (Foundational)**: Depends on Phase 1; blocks all user stories.
- **Phase 3 (US1)**: Depends on Phase 2.
- **Phase 4 (US2)**: Depends on Phase 2.
- **Phase 5 (US3)**: Depends on US1 and US2 for meaningful resilience/cascade tests.
- **Phase 6 (Polish)**: Depends on desired user stories being complete.

### User Story Dependencies

- **US1 (P1)**: Independent after foundational work.
- **US2 (P1)**: Independent after foundational work.
- **US3 (P2)**: Depends on US1/US2 flows existing.

### Within Each User Story

- Write tests first and ensure they fail before implementation.
- Implement contracts/services before UI wiring.
- Complete observability and error handling before story sign-off.

### Parallel Opportunities

- T002 and T004 in setup can run in parallel.
- T007, T008, and T011 in foundational can run in parallel.
- US1 tests T012 and T013 can run in parallel.
- US2 tests T019 through T022 can run in parallel.
- Polish docs tasks T035 through T037 can run in parallel.

---

## Parallel Example: User Story 2

```bash
# Parallel test authoring for US2:
Task: "T019 [US2] Add backend integration test for default/custom limit behavior in backend/tests/SmartHome.Api.IntegrationTests/Devices/DeviceTelemetryHistoryEndpointTests.cs"
Task: "T020 [US2] Add backend integration test for selected-device filtering/order in backend/tests/SmartHome.Api.IntegrationTests/Devices/DeviceTelemetryHistoryEndpointTests.cs"
Task: "T021 [US2] Add frontend details telemetry rendering test in frontend/src/app/features/devices/device-details/device-details.component.spec.ts"
Task: "T022 [US2] Add frontend facade contract-mapping test for telemetry endpoint response in frontend/src/app/features/devices/services/device-pages.facade.spec.ts"

# Parallel implementation once endpoint contract is settled:
Task: "T023 [US2] Add GET /api/devices/{deviceId}/telemetry endpoint with limit validation and OpenAPI metadata in backend/src/SmartHome.Api/api/endpoints/DevicesEndpoints.cs"
Task: "T027 [US2] Add user-selectable telemetry limit state and reload flow in frontend/src/app/features/devices/device-details/device-details.component.ts"
```

---

## Implementation Strategy

### MVP First (US1)

1. Complete Setup and Foundational phases.
2. Deliver US1 realtime update flow.
3. Validate with simulator-driven telemetry ingest.

### Incremental Delivery

1. Deliver US1 realtime device-list updates.
2. Deliver US2 selected-device telemetry endpoint/details list.
3. Deliver US3 resilience and telemetry cleanup validation.
4. Run polish/docs and quickstart verification.

### Parallel Team Strategy

1. Team closes setup/foundation together.
2. Developer A takes US1; Developer B takes US2.
3. Developer C focuses on US3 tests and hardening after US1/US2 are in.

---

## Notes

- [P] tasks target separate files and low dependency overlap.
- [USx] labels provide traceability to user stories.
- Keep backend field names and endpoint paths as source of truth for frontend mapping.
- Validate route composition and end-to-end data flow before diagnosing runtime/framework issues.
