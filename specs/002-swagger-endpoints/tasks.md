# Tasks: Backend Swagger Endpoint Discovery

**Input**: Design documents from /specs/002-swagger-endpoints/

**Prerequisites**: plan.md (required), spec.md (required for user stories), research.md, data-model.md, contracts/

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Prepare package/config/documentation scaffolding for Swagger work.

- [x] T001 Add Swagger package reference in backend/src/SmartHome.Api/SmartHome.Api.csproj
- [x] T002 Add development documentation policy settings in backend/src/SmartHome.Api/appsettings.Development.json
- [x] T003 [P] Add Swagger run and verification commands in docs/development-commands.md

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Enable and gate documentation infrastructure required by all user stories.

**⚠️ CRITICAL**: No user story work starts before this phase is complete.

- [x] T004 Register OpenAPI generation services in backend/src/SmartHome.Api/Program.cs
- [x] T005 Configure Swagger middleware and UI pipeline in backend/src/SmartHome.Api/Program.cs
- [x] T006 Implement restricted-environment docs blocking behavior in backend/src/SmartHome.Api/Program.cs
- [x] T007 [P] Add shared docs policy constants in backend/src/SmartHome.Api/api/middleware/RequestCorrelationMiddleware.cs
- [x] T008 [P] Add diagnostics logging for documentation endpoint access in backend/src/SmartHome.Api/Program.cs
- [x] T009 Add integration host environment toggle support for docs tests in backend/tests/SmartHome.Api.IntegrationTests/Infrastructure/IntegrationTestHost.cs

**Checkpoint**: Swagger infrastructure is available in allowed environments and blocked in restricted environments.

---

## Phase 3: User Story 1 - View API Endpoints in Browser (Priority: P1) 🎯 MVP

**Goal**: Developers can open Swagger UI and discover available endpoints with route/method/schema details.

**Independent Test**: Start backend in development mode, open /swagger, and confirm endpoints are visible with request/response metadata.

### Tests for User Story 1

- [x] T010 [P] [US1] Add integration test for Swagger UI availability in backend/tests/SmartHome.Api.IntegrationTests/Contracts/SwaggerUiAvailabilityTests.cs
- [x] T011 [P] [US1] Add integration test for swagger.json availability in backend/tests/SmartHome.Api.IntegrationTests/Contracts/SwaggerJsonAvailabilityTests.cs

### Implementation for User Story 1

- [x] T012 [US1] Configure Swagger document metadata (title/version/description) in backend/src/SmartHome.Api/Program.cs
- [x] T013 [P] [US1] Add endpoint summaries/tags for device routes in backend/src/SmartHome.Api/api/endpoints/DevicesEndpoints.cs
- [x] T014 [P] [US1] Add endpoint summaries/tags for telemetry routes in backend/src/SmartHome.Api/api/endpoints/TelemetryEndpoints.cs
- [x] T015 [US1] Configure Swagger UI route and OpenAPI endpoint mapping in backend/src/SmartHome.Api/Program.cs
- [x] T016 [US1] Document MVP endpoint-discovery validation steps in specs/002-swagger-endpoints/quickstart.md

**Checkpoint**: User Story 1 is independently functional and demoable.

---

## Phase 4: User Story 2 - Try Endpoints from Documentation (Priority: P2)

**Goal**: Developers can execute requests from docs and review live success/error responses.

**Independent Test**: From Swagger UI, execute a valid and an invalid request; verify response status/body details appear.

### Tests for User Story 2

- [x] T017 [P] [US2] Add contract test verifying requestBody schema for POST /api/devices in backend/tests/SmartHome.Api.IntegrationTests/Contracts/OpenApiContractTests.cs
- [x] T018 [P] [US2] Add contract test verifying documented error/status responses for POST /api/telemetry in backend/tests/SmartHome.Api.IntegrationTests/Contracts/OpenApiContractTests.cs

### Implementation for User Story 2

- [x] T019 [US2] Add response metadata for device registration/listing endpoints in backend/src/SmartHome.Api/api/endpoints/DevicesEndpoints.cs
- [x] T020 [US2] Add response metadata for telemetry ingest/latest endpoints in backend/src/SmartHome.Api/api/endpoints/TelemetryEndpoints.cs
- [x] T021 [US2] Ensure schema generation for request/response DTOs in backend/src/SmartHome.Api/Program.cs
- [x] T022 [US2] Add request execution and error-visibility validation steps in specs/002-swagger-endpoints/quickstart.md

**Checkpoint**: User Story 2 is independently functional with visible response/error details.

---

## Phase 5: User Story 3 - Keep Docs Aligned with API Changes (Priority: P3)

**Goal**: Swagger output stays synchronized with API route metadata and environment policy behavior.

**Independent Test**: Change endpoint metadata and verify generated docs/tests update consistently in the same cycle.

### Tests for User Story 3

- [x] T023 [P] [US3] Replace OpenAPI smoke test with required-path assertions in backend/tests/SmartHome.Api.IntegrationTests/Contracts/OpenApiContractTests.cs
- [x] T024 [P] [US3] Add restricted-environment docs unavailability test in backend/tests/SmartHome.Api.IntegrationTests/Contracts/SwaggerRestrictedModeTests.cs

### Implementation for User Story 3

- [x] T025 [US3] Add stable operationId metadata for device endpoints in backend/src/SmartHome.Api/api/endpoints/DevicesEndpoints.cs
- [x] T026 [US3] Add stable operationId metadata for telemetry endpoints in backend/src/SmartHome.Api/api/endpoints/TelemetryEndpoints.cs
- [x] T027 [US3] Update docs access contract snapshot in specs/002-swagger-endpoints/contracts/openapi-docs.yaml
- [x] T028 [US3] Add contract-drift check guidance for docs sync in specs/002-swagger-endpoints/research.md

**Checkpoint**: User Story 3 is independently functional with regression protection for docs drift.

---

## Phase 6: Polish & Cross-Cutting Concerns

**Purpose**: Final hardening, docs alignment, and verification across stories.

- [x] T029 [P] Add Swagger verification checklist to docs/mvp-readiness.md
- [x] T030 Run quickstart end-to-end and capture validated outcomes in specs/002-swagger-endpoints/quickstart.md
- [x] T031 [P] Add docs endpoint observability notes in specs/002-swagger-endpoints/plan.md

---

## Dependencies & Execution Order

### Phase Dependencies

- Setup (Phase 1): No dependencies; start immediately.
- Foundational (Phase 2): Depends on Phase 1; blocks all user stories.
- User Story phases (Phase 3-5): Depend on Phase 2 completion.
- Polish (Phase 6): Depends on completion of selected user stories.

### User Story Dependencies

- US1 (P1): Starts after Foundational; no dependency on other stories.
- US2 (P2): Starts after Foundational; relies on docs UI being present but remains independently testable.
- US3 (P3): Starts after Foundational; validates synchronization and environment behavior and can run independently of US2.

### Within Each User Story

- Tests first and failing before implementation.
- Endpoint metadata updates before quickstart/contract documentation updates.
- Story-level checkpoint must pass before moving to next priority for incremental delivery.

## Parallel Opportunities

- Setup: T003 can run parallel with T001-T002.
- Foundational: T007 and T008 can run parallel after T004 starts; T009 runs in parallel once test host context is ready.
- US1: T010 and T011 parallel; T013 and T014 parallel.
- US2: T017 and T018 parallel.
- US3: T023 and T024 parallel.
- Polish: T029 and T031 parallel.

## Parallel Example: User Story 1

- Run T010 and T011 together (UI and JSON availability tests).
- Run T013 and T014 together (devices and telemetry endpoint metadata).

## Parallel Example: User Story 2

- Run T017 and T018 together (OpenAPI contract assertions for request/response behavior).

## Parallel Example: User Story 3

- Run T023 and T024 together (docs drift and restricted-environment behavior coverage).

## Implementation Strategy

### MVP First (US1 only)

1. Complete Phase 1 (Setup).
2. Complete Phase 2 (Foundational).
3. Complete Phase 3 (US1).
4. Validate US1 independently by running backend and opening /swagger.

### Incremental Delivery

1. Ship US1 for endpoint discovery value.
2. Add US2 for interactive request execution and response visibility.
3. Add US3 for synchronization and restricted-environment hardening.
4. Finish with Phase 6 polish and full quickstart validation.
