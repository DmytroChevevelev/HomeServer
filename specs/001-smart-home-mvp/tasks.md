# Tasks: Smart Home MVP Telemetry Flow

**Input**: Design documents from `/specs/001-smart-home-mvp/`
**Prerequisites**: plan.md (required), spec.md (required for user stories), research.md, data-model.md, contracts/, quickstart.md

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Initialize repository structure, toolchain, and baseline configuration for backend, frontend, and simulator.

- [X] T001 Create backend API project skeleton in backend/src/SmartHome.Api/SmartHome.Api.csproj
- [X] T002 Create backend test projects in backend/tests/SmartHome.Api.UnitTests/SmartHome.Api.UnitTests.csproj and backend/tests/SmartHome.Api.IntegrationTests/SmartHome.Api.IntegrationTests.csproj
- [X] T003 [P] Create Angular application scaffold in frontend/package.json
- [X] T004 [P] Create simulator console project in simulator/src/SmartHome.Simulator/SmartHome.Simulator.csproj
- [X] T005 Configure backend app settings template for SQL and CORS in backend/src/SmartHome.Api/appsettings.Development.json
- [X] T006 Configure frontend environment API base URL in frontend/src/environments/environment.ts
- [X] T007 [P] Add root-level development commands documentation in docs/development-commands.md

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Build core cross-story infrastructure that must exist before story implementation.

- [X] T008 Create Device and TelemetryReading entities in backend/src/SmartHome.Api/models/Device.cs and backend/src/SmartHome.Api/models/TelemetryReading.cs
- [X] T009 Create EF Core DbContext and model configuration in backend/src/SmartHome.Api/infrastructure/SmartHomeDbContext.cs
- [X] T010 Create initial EF Core migration for device and telemetry schema in backend/src/SmartHome.Api/infrastructure/migrations/0001_Initial.cs
- [X] T011 [P] Implement structured ValidationError response model and factory in backend/src/SmartHome.Api/api/errors/ValidationErrorFactory.cs
- [X] T012 [P] Implement request correlation middleware and structured request logging in backend/src/SmartHome.Api/api/middleware/RequestCorrelationMiddleware.cs
- [X] T013 Implement global exception and validation handling middleware in backend/src/SmartHome.Api/api/middleware/ErrorHandlingMiddleware.cs
- [X] T014 [P] Configure CORS policy and environment-driven settings in backend/src/SmartHome.Api/api/Program.cs
- [X] T015 [P] Add API contract baseline file and schema alignment notes in backend/src/SmartHome.Api/api/contracts/openapi.yaml
- [X] T016 Implement telemetry status evaluation utility (5-minute rule + never-reported stale) in backend/src/SmartHome.Api/services/DeviceStatusEvaluator.cs
- [X] T017 [P] Add backend test fixture for SQL-backed integration tests in backend/tests/SmartHome.Api.IntegrationTests/Infrastructure/IntegrationTestHost.cs

**Checkpoint**: Foundation ready - user story implementation can begin.

---

## Phase 3: User Story 1 - Register and View Devices (Priority: P1) 🎯 MVP

**Goal**: Operators can register devices and view them in the device list.

**Independent Test**: Register a valid device, verify it appears in listing; submit invalid and duplicate registrations and verify structured errors.

### Tests for User Story 1

- [X] T018 [P] [US1] Add integration test for successful device registration in backend/tests/SmartHome.Api.IntegrationTests/Devices/RegisterDeviceTests.cs
- [X] T019 [P] [US1] Add integration test for duplicate externalId rejection in backend/tests/SmartHome.Api.IntegrationTests/Devices/RegisterDeviceDuplicateTests.cs
- [X] T020 [P] [US1] Add integration test for validation error payload shape in backend/tests/SmartHome.Api.IntegrationTests/Devices/RegisterDeviceValidationTests.cs
- [X] T021 [P] [US1] Add frontend component test for device registration form validation in frontend/src/app/features/devices/device-registration/device-registration.component.spec.ts

### Implementation for User Story 1

- [X] T022 [US1] Implement device registration request/response DTOs in backend/src/SmartHome.Api/api/contracts/DevicesContracts.cs
- [X] T023 [US1] Implement POST /devices endpoint with duplicate checks in backend/src/SmartHome.Api/api/endpoints/DevicesEndpoints.cs
- [X] T024 [US1] Implement GET /devices endpoint with latest status projection in backend/src/SmartHome.Api/api/endpoints/DevicesEndpoints.cs
- [X] T025 [P] [US1] Implement device repository queries for create/list operations in backend/src/SmartHome.Api/infrastructure/repositories/DeviceRepository.cs
- [X] T026 [P] [US1] Implement Angular device API client service in frontend/src/app/features/devices/services/devices-api.service.ts
- [X] T027 [US1] Implement Angular device registration form component in frontend/src/app/features/devices/device-registration/device-registration.component.ts
- [X] T028 [US1] Implement Angular device list component with status column in frontend/src/app/features/devices/device-list/device-list.component.ts
- [X] T029 [US1] Wire device feature routes and page shell in frontend/src/app/features/devices/devices.routes.ts

**Checkpoint**: User Story 1 independently functional and testable.

---

## Phase 4: User Story 2 - Ingest Telemetry Readings (Priority: P2)

**Goal**: Telemetry is accepted for registered devices, validated, persisted, and resilient to out-of-order events.

**Independent Test**: Submit telemetry for known/unknown devices, verify acceptance/rejection and persistence including out-of-order event handling.

### Tests for User Story 2

- [X] T030 [P] [US2] Add integration test for successful telemetry ingestion in backend/tests/SmartHome.Api.IntegrationTests/Telemetry/IngestTelemetryTests.cs
- [X] T031 [P] [US2] Add integration test for unknown device rejection (404 + error contract) in backend/tests/SmartHome.Api.IntegrationTests/Telemetry/IngestUnknownDeviceTests.cs
- [X] T032 [P] [US2] Add integration test for out-of-order telemetry acceptance in backend/tests/SmartHome.Api.IntegrationTests/Telemetry/IngestOutOfOrderTelemetryTests.cs
- [X] T033 [P] [US2] Add unit test for ingestion throttling response mapping in backend/tests/SmartHome.Api.UnitTests/Telemetry/IngestionThrottleTests.cs

### Implementation for User Story 2

- [X] T034 [US2] Implement telemetry ingestion DTOs and validators in backend/src/SmartHome.Api/api/contracts/TelemetryContracts.cs
- [X] T035 [US2] Implement POST /telemetry endpoint with structured 400/404/429 responses in backend/src/SmartHome.Api/api/endpoints/TelemetryEndpoints.cs
- [X] T036 [P] [US2] Implement telemetry persistence service using eventTimeUtc semantics in backend/src/SmartHome.Api/services/TelemetryIngestionService.cs
- [X] T037 [P] [US2] Implement telemetry repository write/read helpers in backend/src/SmartHome.Api/infrastructure/repositories/TelemetryRepository.cs
- [X] T038 [US2] Implement lightweight ingestion throttling guard in backend/src/SmartHome.Api/api/middleware/IngestionRateLimiterMiddleware.cs
- [X] T039 [P] [US2] Implement simulator telemetry payload generator and sender in simulator/src/SmartHome.Simulator/TelemetryPublisher.cs
- [X] T040 [US2] Add simulator runtime configuration for API URL and interval in simulator/src/SmartHome.Simulator/appsettings.json

**Checkpoint**: User Stories 1 and 2 independently functional.

---

## Phase 5: User Story 3 - Monitor Latest Telemetry in Dashboard (Priority: P3)

**Goal**: Operators can view latest telemetry and active/stale status in near real-time.

**Independent Test**: Ingest telemetry, open dashboard, verify latest values and stale transition behavior after 5 minutes.

### Tests for User Story 3

- [X] T041 [P] [US3] Add integration test for GET /telemetry/latest projection semantics in backend/tests/SmartHome.Api.IntegrationTests/Telemetry/GetLatestTelemetryTests.cs
- [X] T042 [P] [US3] Add unit test for active/stale threshold and never-reported behavior in backend/tests/SmartHome.Api.UnitTests/Devices/DeviceStatusEvaluatorTests.cs
- [X] T043 [P] [US3] Add frontend component test for telemetry dashboard rendering in frontend/src/app/features/dashboard/telemetry-dashboard/telemetry-dashboard.component.spec.ts

### Implementation for User Story 3

- [X] T044 [US3] Implement GET /telemetry/latest endpoint in backend/src/SmartHome.Api/api/endpoints/TelemetryEndpoints.cs
- [X] T045 [P] [US3] Implement latest telemetry query projection service in backend/src/SmartHome.Api/services/LatestTelemetryQueryService.cs
- [X] T046 [P] [US3] Implement Angular telemetry API client in frontend/src/app/features/dashboard/services/telemetry-api.service.ts
- [X] T047 [US3] Implement Angular telemetry dashboard component with status badges in frontend/src/app/features/dashboard/telemetry-dashboard/telemetry-dashboard.component.ts
- [X] T048 [US3] Implement dashboard auto-refresh polling and error state handling in frontend/src/app/features/dashboard/telemetry-dashboard/telemetry-dashboard.component.ts
- [X] T049 [US3] Wire dashboard route and navigation entry in frontend/src/app/app.routes.ts and frontend/src/app/core/navigation/navigation.component.ts

**Checkpoint**: All user stories independently functional and integrated.

---

## Phase 6: Polish & Cross-Cutting Concerns

**Purpose**: Harden quality, documentation, and operational readiness across all stories.

- [X] T050 [P] Add contract test coverage against OpenAPI schemas in backend/tests/SmartHome.Api.IntegrationTests/Contracts/OpenApiContractTests.cs
- [X] T051 [P] Add end-to-end quickstart verification script in scripts/validate-mvp.ps1
- [X] T052 Improve structured logging fields and correlation propagation in backend/src/SmartHome.Api/api/middleware/RequestCorrelationMiddleware.cs
- [X] T053 [P] Update onboarding and run instructions in specs/001-smart-home-mvp/quickstart.md
- [X] T054 [P] Add release-readiness checklist for MVP demo in docs/mvp-readiness.md

---

## Dependencies & Execution Order

### Phase Dependencies

- Setup (Phase 1): starts immediately.
- Foundational (Phase 2): depends on Setup and blocks all story work.
- User Stories (Phases 3-5): depend on Foundational completion.
- Polish (Phase 6): depends on selected user stories completion.

### User Story Dependencies

- US1 (P1): starts after Foundational; independent MVP entry point.
- US2 (P2): starts after Foundational; depends logically on device registration data but remains independently testable with seeded device.
- US3 (P3): starts after Foundational; depends on telemetry ingestion capability from US2 for meaningful data display.

### Within Each User Story

- Tests first (where defined), then models/services, then endpoints/UI wiring, then integration polish.

## Parallel Opportunities

- Setup: T003, T004, T007 can run in parallel after T001-T002.
- Foundational: T011, T012, T014, T015, T017 can run in parallel once T008-T010 begin.
- US1: T018-T021 and T025-T026 can run in parallel.
- US2: T030-T033 and T036-T037-T039 can run in parallel.
- US3: T041-T043 and T045-T046 can run in parallel.
- Polish: T050, T051, T053, T054 can run in parallel.

## Parallel Example: User Story 1

```bash
# Parallel test tasks
T018 Register device integration test
T019 Duplicate externalId integration test
T020 Validation error contract integration test
T021 Frontend registration component test

# Parallel implementation tasks
T025 Device repository implementation
T026 Angular device API client service
```

## Parallel Example: User Story 2

```bash
# Parallel test tasks
T030 Successful ingestion integration test
T031 Unknown device rejection integration test
T032 Out-of-order ingestion integration test
T033 Ingestion throttling unit test

# Parallel implementation tasks
T036 Telemetry ingestion service
T037 Telemetry repository helpers
T039 Simulator telemetry publisher
```

## Parallel Example: User Story 3

```bash
# Parallel test tasks
T041 Latest telemetry projection integration test
T042 Status evaluation unit test
T043 Dashboard rendering component test

# Parallel implementation tasks
T045 Latest telemetry query service
T046 Angular telemetry API client
```

## Implementation Strategy

### MVP First (US1 only)

1. Complete Phase 1 and Phase 2.
2. Deliver Phase 3 (US1) fully.
3. Validate registration and device listing as standalone MVP increment.

### Incremental Delivery

1. Add US2 to enable telemetry ingestion and persistence.
2. Add US3 to enable dashboard monitoring and stale status visualization.
3. Run quickstart validation and contract tests before demo/release.

### Team Parallelization

1. Developer A: backend endpoints and persistence.
2. Developer B: frontend features and UI tests.
3. Developer C: simulator and integration/contract test harness.

