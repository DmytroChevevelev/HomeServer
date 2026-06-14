# Tasks: Database Sync and API Docs Metadata

**Input**: Design documents from /specs/003-update-db-docs/

**Prerequisites**: plan.md (required), spec.md (required for user stories), research.md, data-model.md, contracts/

**Tests**: Automated tests are required by this feature specification for migration safety and OpenAPI contract/doc consistency.

**Organization**: Tasks are grouped by user story to enable independent implementation and testing.

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Prepare local tooling and developer documentation needed for implementation.

- [X] T001 Add database synchronization commands in docs/development-commands.md
- [X] T002 Document database sync setup workflow, prerequisites, and operator-run command path in backend/README.md
- [X] T003 [P] Add root-level cross-component feature summary for DB sync and docs checks in README.md
- [X] T004 [P] Document supported environment matrix (local dev + CI) in specs/003-update-db-docs/quickstart.md

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Core prerequisites that block all user stories until complete.

**⚠️ CRITICAL**: No user story work starts before this phase is complete.

- [X] T005 Add migration-state diagnostics flow (without startup auto-apply) in backend/src/SmartHome.Api/Program.cs
- [X] T006 [P] Add database and documentation feature flags in backend/src/SmartHome.Api/appsettings.Development.json
- [X] T007 [P] Add reusable database host setup support for migration scenarios in backend/tests/SmartHome.Api.IntegrationTests/Infrastructure/IntegrationTestHost.cs
- [X] T008 Add shared OpenAPI metadata assertion helpers for contract tests in backend/tests/SmartHome.Api.IntegrationTests/Contracts/OpenApiContractTests.cs

**Checkpoint**: Foundation ready, user stories can start.

---

## Phase 3: User Story 1 - Initialize and Update Database State (Priority: P1) 🎯 MVP

**Goal**: Developers can create a fresh database and update existing schemas to current version through a repeatable migration flow.

**Independent Test**: Execute migration update against empty and existing databases, then verify API data access succeeds.

### Tests for User Story 1

- [X] T009 [P] [US1] Add integration test for database creation from empty state in backend/tests/SmartHome.Api.IntegrationTests/Infrastructure/DatabaseMigrationTests.cs
- [X] T010 [P] [US1] Add integration test for idempotent schema upgrade on existing database in backend/tests/SmartHome.Api.IntegrationTests/Infrastructure/DatabaseMigrationUpgradeTests.cs
- [X] T011 [P] [US1] Add integration test proving operational data is preserved during migration update in backend/tests/SmartHome.Api.IntegrationTests/Infrastructure/DatabaseMigrationDataPreservationTests.cs

### Implementation for User Story 1

- [X] T012 [US1] Implement operator-run migration guardrails and startup warnings (without automatic migration execution) in backend/src/SmartHome.Api/Program.cs
- [X] T013 [P] [US1] Add migration script for local create/update workflow in scripts/apply-db-migrations.ps1
- [X] T014 [US1] Evaluate whether schema changes are required and complete one branch with explicit evidence in specs/003-update-db-docs/research.md: (A) create and validate a new EF migration if model changes exist, including migration file name and `dotnet ef database update` output summary; or (B) record a no-migration-needed decision with `dotnet ef migrations list` output summary and successful `dotnet ef database update` output summary
- [X] T015 [US1] Document migration verification steps, rollback notes, and data-preservation checks in specs/003-update-db-docs/quickstart.md

**Checkpoint**: User Story 1 is independently testable and demoable.

---

## Phase 4: User Story 2 - Document Endpoint Behavior in OpenAPI (Priority: P2)

**Goal**: Each changed endpoint exposes summary, parameter descriptions when applicable, and response descriptions in generated docs.

**Independent Test**: Inspect generated swagger.json and confirm required operation metadata for each changed endpoint.

### Tests for User Story 2

- [X] T016 [P] [US2] Add devices endpoint metadata contract tests in backend/tests/SmartHome.Api.IntegrationTests/Contracts/OpenApiDevicesMetadataTests.cs
- [X] T017 [P] [US2] Add telemetry endpoint metadata contract tests in backend/tests/SmartHome.Api.IntegrationTests/Contracts/OpenApiTelemetryMetadataTests.cs
- [X] T018 [P] [US2] Add test validating contract XML/doc comments are surfaced in generated OpenAPI output in backend/tests/SmartHome.Api.IntegrationTests/Contracts/OpenApiContractCommentVisibilityTests.cs
- [X] T019 [P] [US2] Add test verifying updated route behavior is inferable from OpenAPI docs alone in backend/tests/SmartHome.Api.IntegrationTests/Contracts/OpenApiBehaviorDocumentationTests.cs

### Implementation for User Story 2

- [X] T020 [US2] Add/update OpenAPI summaries, parameter descriptions, and response descriptions in backend/src/SmartHome.Api/api/endpoints/DevicesEndpoints.cs
- [X] T021 [US2] Add/update OpenAPI summaries, parameter descriptions, and response descriptions in backend/src/SmartHome.Api/api/endpoints/TelemetryEndpoints.cs
- [X] T022 [US2] Add documentation comments for modified request/response contracts in backend/src/SmartHome.Api/api/contracts/DevicesContracts.cs
- [X] T023 [US2] Add documentation comments for modified request/response contracts in backend/src/SmartHome.Api/api/contracts/TelemetryContracts.cs
- [X] T024 [US2] Update endpoint documentation metadata usage section (summaries, parameter descriptions, response descriptions) in backend/README.md

**Checkpoint**: User Story 2 is independently testable and complete.

---

## Phase 5: User Story 3 - Keep Contracts and Endpoint Docs Consistent (Priority: P3)

**Goal**: Contract artifacts and generated endpoint docs remain aligned, with automated drift detection.

**Independent Test**: Run contract tests that validate route metadata and docs route behavior against source contracts.

### Tests for User Story 3

- [X] T025 [P] [US3] Add docs route parity contract test against source contract in backend/tests/SmartHome.Api.IntegrationTests/Contracts/SwaggerContractParityTests.cs
- [X] T026 [P] [US3] Add restricted-environment docs behavior regression tests in backend/tests/SmartHome.Api.IntegrationTests/Contracts/SwaggerRestrictedModeTests.cs

### Implementation for User Story 3

- [X] T027 [US3] Align feature contract metadata requirements in specs/003-update-db-docs/contracts/openapi-docs-alignment.yaml
- [X] T028 [US3] Align docs access source contract expectations in specs/002-swagger-endpoints/contracts/openapi-docs.yaml
- [X] T029 [US3] Update documentation route handling and diagnostics notes for contract consistency in backend/src/SmartHome.Api/Program.cs
- [X] T030 [US3] Add contract drift validation guidance in specs/003-update-db-docs/research.md

**Checkpoint**: User Story 3 is independently testable with contract drift protection.

---

## Phase 6: Polish & Cross-Cutting Concerns

**Purpose**: Final quality, documentation alignment, and full validation.

- [X] T031 [P] Update MVP readiness criteria for DB sync and docs metadata in docs/mvp-readiness.md
- [X] T032 [P] Add CI schema-sync validation step to scripts/validate-mvp.ps1 and assert migration state is current
- [X] T033 Run backend integration test suite and document outcomes in specs/003-update-db-docs/quickstart.md
- [X] T034 Verify and update observability/configuration notes after implementation in specs/003-update-db-docs/plan.md

---

## Dependencies & Execution Order

### Phase Dependencies

- Setup (Phase 1): No dependencies.
- Foundational (Phase 2): Depends on Setup completion; blocks all user stories.
- User Story phases (Phase 3-5): Depend on Foundational completion.
- Polish (Phase 6): Depends on selected user stories being complete.

### User Story Dependencies

- US1 (P1): Starts after Foundational; no dependency on US2 or US3.
- US2 (P2): Starts after Foundational; can proceed independently of US1 once shared foundation is done.
- US3 (P3): Starts after Foundational; validates cross-artifact consistency and can run independently of US2 implementation details.

### Within Each User Story

- Write tests first and ensure they fail before implementation tasks.
- Implement endpoint/contract changes after tests are in place.
- Update feature docs and quickstart before story checkpoint sign-off.

## Parallel Opportunities

- Setup: T003 and T004 can run parallel with T001-T002.
- Foundational: T006 and T007 can run in parallel after T005 starts.
- US1: T009, T010, and T011 run in parallel; T013 runs in parallel with T012.
- US2: T016, T017, T018, and T019 run in parallel.
- US3: T025 and T026 run in parallel.
- Polish: T031 and T032 run in parallel after implementation tasks are complete.

## Parallel Example: User Story 1

- Run T009, T010, and T011 together to validate empty, upgrade, and data-preservation scenarios.
- Run T013 in parallel while T012 is implemented.

## Parallel Example: User Story 2

- Run T016, T017, T018, and T019 together to validate endpoint metadata, contract-comment visibility, and docs-only behavior understanding.

## Parallel Example: User Story 3

- Run T025 and T026 together to validate contract parity and restricted-mode behavior.

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1 and Phase 2.
2. Complete Phase 3 (US1).
3. Validate DB create/update flow independently.
4. Demo MVP migration reliability.

### Incremental Delivery

1. Deliver US1 (database reliability).
2. Deliver US2 (operation-level docs quality).
3. Deliver US3 (contract consistency and drift detection).
4. Complete polish tasks and final verification.
