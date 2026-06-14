# Tasks: Fix Devices CORS

**Input**: Design documents from `/specs/007-fix-devices-cors/`

**Prerequisites**: plan.md (required), spec.md (required for user stories), research.md, data-model.md, contracts/

**Tests**: Automated tests are required by spec FR-006; include integration tests for allowed and disallowed origins.

**Organization**: Tasks are grouped by user story to enable independent implementation and testing of each story.

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Prepare files and baseline checks for CORS policy changes.

- [x] T001 Verify active feature context points to `specs/007-fix-devices-cors` in `.specify/feature.json`
- [x] T002 Capture current CORS behavior baseline for `/api/devices` in `backend/tests/SmartHome.Api.IntegrationTests/Contracts/` by reviewing existing test host usage patterns

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Establish shared CORS test scaffolding and policy validation helpers before user story implementation.

**⚠️ CRITICAL**: No user story implementation should proceed until this phase is complete.

- [x] T003 Create shared CORS assertion helper for response headers in `backend/tests/SmartHome.Api.IntegrationTests/Infrastructure/` (new helper file)
- [x] T004 [P] Add origin-based request utility for integration tests in `backend/tests/SmartHome.Api.IntegrationTests/Infrastructure/` (new helper file)
- [x] T005 Confirm backend CORS policy remains explicit `WithOrigins(...)` in `backend/src/SmartHome.Api/Program.cs` and document expectation in test comments

**Checkpoint**: Foundational test helpers ready and CORS policy guardrails established.

---

## Phase 3: User Story 1 - Frontend Loads Devices from Supported Dev Origins (Priority: P1) 🎯 MVP

**Goal**: Allow browser device-list requests from supported local development origins.

**Independent Test**: Requests to `/api/devices` from `http://localhost:4200`, `http://localhost:4201`, and `http://127.0.0.1:4200` include valid CORS allow headers.

### Tests for User Story 1

- [x] T006 [P] [US1] Add integration test for allowed origin `http://localhost:4200` on `/api/devices` in `backend/tests/SmartHome.Api.IntegrationTests/Contracts/CorsDevicesOriginTests.cs`
- [x] T007 [P] [US1] Add integration test for allowed origin `http://localhost:4201` on `/api/devices` in `backend/tests/SmartHome.Api.IntegrationTests/Contracts/CorsDevicesOriginTests.cs`
- [x] T008 [P] [US1] Add integration test for allowed origin `http://127.0.0.1:4200` on `/api/devices` in `backend/tests/SmartHome.Api.IntegrationTests/Contracts/CorsDevicesOriginTests.cs`

### Implementation for User Story 1

- [x] T009 [US1] Update `Cors:AllowedOrigins` development defaults in `backend/src/SmartHome.Api/appsettings.Development.json` to include `http://localhost:4201` and `http://127.0.0.1:4200`
- [x] T010 [US1] Add/adjust XML documentation comments for modified CORS-relevant endpoint behavior in `backend/src/SmartHome.Api/api/endpoints/DevicesEndpoints.cs` to keep OpenAPI summary/response metadata aligned
- [x] T011 [US1] Ensure endpoint documentation metadata remains explicit in `backend/src/SmartHome.Api/api/endpoints/DevicesEndpoints.cs` (`WithSummary`, `WithDescription`, `WithOpenApi` response descriptions)

**Checkpoint**: P1 is independently functional and testable with supported local origins.

---

## Phase 4: User Story 2 - Maintain Strict Origin Controls (Priority: P2)

**Goal**: Keep explicit origin restrictions and verify unsupported origins are not granted cross-origin access.

**Independent Test**: Unsupported origin requests do not receive `Access-Control-Allow-Origin` while supported origins continue to pass.

### Tests for User Story 2

- [x] T012 [P] [US2] Add integration test for disallowed origin `http://localhost:9999` on `/api/devices` in `backend/tests/SmartHome.Api.IntegrationTests/Contracts/CorsDevicesOriginTests.cs`
- [x] T013 [P] [US2] Add integration test asserting allow header is absent for disallowed origins in `backend/tests/SmartHome.Api.IntegrationTests/Contracts/CorsDevicesOriginTests.cs`
- [x] T014 [P] [US2] Add regression test asserting previously allowed origin behavior remains unchanged in `backend/tests/SmartHome.Api.IntegrationTests/Contracts/CorsDevicesOriginTests.cs`

### Implementation for User Story 2

- [x] T015 [US2] Review and keep explicit allowlist policy logic unchanged in `backend/src/SmartHome.Api/Program.cs` (no wildcard origin behavior)
- [x] T016 [US2] Add configuration safety comments or validation notes around `Cors:AllowedOrigins` handling in `backend/src/SmartHome.Api/Program.cs`

**Checkpoint**: P2 constraints validated and strict explicit origin policy preserved.

---

## Phase 5: User Story 3 - Fast CORS Troubleshooting for Developers (Priority: P3)

**Goal**: Provide clear developer documentation for supported origins and troubleshooting steps.

**Independent Test**: A developer can resolve a blocked local `/api/devices` fetch using documentation only.

### Implementation for User Story 3

- [x] T017 [US3] Update local-origin and CORS troubleshooting guidance in `docs/development-commands.md`
- [x] T018 [US3] Add CORS troubleshooting note for frontend/backend startup in `backend/README.md`
- [x] T019 [US3] Add a short API-consumer note on origin requirements in `README.md`

**Checkpoint**: P3 documentation enables self-service diagnosis and resolution.

---

## Phase 6: Polish & Cross-Cutting Concerns

**Purpose**: Final validation and cleanup across stories.

- [x] T020 [P] Run integration test suite for contracts in `backend/tests/SmartHome.Api.IntegrationTests/`
- [x] T021 Run targeted contract tests including `CorsDevicesOriginTests` and existing swagger contract tests in `backend/tests/SmartHome.Api.IntegrationTests/Contracts/`
- [x] T022 [P] Validate quickstart workflow in `specs/007-fix-devices-cors/quickstart.md` against local run steps
- [x] T023 Confirm no payload/schema changes to `/api/devices` by re-checking `specs/001-smart-home-mvp/contracts/openapi.yaml`

---

## Dependencies & Execution Order

### Phase Dependencies

- **Phase 1 (Setup)**: Starts immediately
- **Phase 2 (Foundational)**: Depends on Phase 1 and blocks user stories
- **Phase 3 (US1)**: Depends on Phase 2
- **Phase 4 (US2)**: Depends on Phase 3 baseline tests and config updates
- **Phase 5 (US3)**: Can run after Phase 3 once behavior is finalized
- **Phase 6 (Polish)**: Depends on completion of all selected user stories

### User Story Dependencies

- **US1 (P1)**: No user-story dependency; establishes core fix and MVP
- **US2 (P2)**: Depends on US1 origin updates and test scaffolding
- **US3 (P3)**: Depends on finalized behavior from US1/US2 for accurate documentation

### Parallel Opportunities

- In US1, T006-T008 can run in parallel
- In US2, T012-T014 can run in parallel
- In final polish, T020 and T022 can run in parallel

---

## Implementation Strategy

### MVP First (US1 Only)

1. Complete Setup + Foundational phases
2. Deliver US1 tasks T006-T011
3. Validate supported local origins work end-to-end

### Incremental Delivery

1. Ship MVP (US1)
2. Add strict-origin regression coverage (US2)
3. Add documentation and troubleshooting guidance (US3)
4. Execute polish validation tasks before merge
