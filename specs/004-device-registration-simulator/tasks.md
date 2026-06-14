# Tasks: Simulator Device Registration Commands

**Input**: Design documents from `/specs/004-device-registration-simulator/`

**Prerequisites**: plan.md (required), spec.md (required for user stories), research.md, data-model.md, contracts/

**Tests**: Tests are included because the specification explicitly requires minimum automated coverage for command parsing, state transitions, interval defaults, and telemetry profile loading.

**Organization**: Tasks are grouped by user story to enable independent implementation and testing of each story.

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Prepare simulator testing and runtime scaffolding used by all stories.

- [X] T001 Create simulator test project and test folders in simulator/tests/SmartHome.Simulator.Tests/SmartHome.Simulator.Tests.csproj
- [X] T002 Add simulator test project references and test dependencies in simulator/tests/SmartHome.Simulator.Tests/SmartHome.Simulator.Tests.csproj
- [X] T003 [P] Add telemetry profile fixture files for device-based test scenarios in simulator/tests/SmartHome.Simulator.Tests/fixtures/device-001.json
- [X] T004 [P] Add a command/runtime section documenting supported commands and profile file naming in simulator/README.md

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Establish core runtime abstractions that all user stories depend on.

**⚠️ CRITICAL**: No user story work can begin until this phase is complete.

- [X] T005 Define simulator runtime models for device profile, command input, and telemetry profile in simulator/src/SmartHome.Simulator/Runtime/SimulatorModels.cs
- [X] T006 Define telemetry session state enum and transition guard helpers in simulator/src/SmartHome.Simulator/Runtime/TelemetrySessionState.cs
- [X] T007 Implement appsettings parsing with 2-second default interval fallback in simulator/src/SmartHome.Simulator/Runtime/SimulatorConfigurationLoader.cs
- [X] T008 Implement telemetry profile file resolver by device-name convention in simulator/src/SmartHome.Simulator/Runtime/TelemetryProfileLoader.cs
- [X] T009 Implement backend request client for device registration and telemetry submission in simulator/src/SmartHome.Simulator/Runtime/SimulatorBackendClient.cs
- [X] T010 [P] Add foundational unit tests for configuration loader and profile loader validation rules in simulator/tests/SmartHome.Simulator.Tests/configuration/SimulatorConfigurationLoaderTests.cs
- [X] T011 [P] Add foundational unit tests for telemetry session state transitions in simulator/tests/SmartHome.Simulator.Tests/telemetry/TelemetrySessionStateTests.cs

**Checkpoint**: Foundation ready - user story implementation can now begin.

---

## Phase 3: User Story 1 - Register A Simulator Device (Priority: P1) 🎯 MVP

**Goal**: Operator can enter `register-device` and get clear success/failure feedback from backend registration.

**Independent Test**: Run simulator, enter `register-device`, and verify registration request/response handling without starting telemetry.

### Tests for User Story 1

- [X] T012 [P] [US1] Add integration test for successful device registration command flow in simulator/tests/SmartHome.Simulator.Tests/command/RegisterDeviceCommandTests.cs
- [X] T013 [P] [US1] Add integration test for duplicate or backend-unavailable registration feedback in simulator/tests/SmartHome.Simulator.Tests/command/RegisterDeviceCommandErrorTests.cs

### Implementation for User Story 1

- [X] T014 [US1] Implement register-device command handler using backend client and structured console feedback in simulator/src/SmartHome.Simulator/Commands/RegisterDeviceCommandHandler.cs
- [X] T015 [US1] Refactor simulator startup to command loop dispatch with register-device support in simulator/src/SmartHome.Simulator/Program.cs
- [X] T016 [US1] Update simulator README usage section with register-device command examples in simulator/README.md

**Checkpoint**: User Story 1 is independently functional and testable.

---

## Phase 4: User Story 2 - Control Telemetry From The Console (Priority: P2)

**Goal**: Operator can start and stop telemetry in the same process with duplicate-loop prevention.

**Independent Test**: Enter `start-send-telemetry` then `stop-send-telemetry` and confirm one active send loop is started then cleanly stopped.

### Tests for User Story 2

- [X] T017 [P] [US2] Add test for start-send-telemetry command creating exactly one active send loop in simulator/tests/SmartHome.Simulator.Tests/command/StartTelemetryCommandTests.cs
- [X] T018 [P] [US2] Add test for stop-send-telemetry when telemetry is active and when idle in simulator/tests/SmartHome.Simulator.Tests/command/StopTelemetryCommandTests.cs
- [X] T019 [P] [US2] Add test for duplicate start-send-telemetry command feedback and no-op behavior in simulator/tests/SmartHome.Simulator.Tests/command/StartTelemetryDuplicateTests.cs

### Implementation for User Story 2

- [X] T020 [US2] Refactor telemetry publisher to support cancellable start/stop lifecycle instead of startup-only infinite loop in simulator/src/SmartHome.Simulator/TelemetryPublisher.cs
- [X] T021 [US2] Implement start-send-telemetry and stop-send-telemetry command handlers with state-guarded transitions in simulator/src/SmartHome.Simulator/Commands/TelemetryControlCommandHandlers.cs
- [X] T022 [US2] Extend command loop dispatch to include start-send-telemetry and stop-send-telemetry routing in simulator/src/SmartHome.Simulator/Program.cs
- [X] T023 [US2] Add command-level observability messages for start, stop, and invalid-state transitions in simulator/src/SmartHome.Simulator/Program.cs
- [X] T024 [US2] Document start/stop command behavior and interval default fallback examples in simulator/README.md

**Checkpoint**: User Stories 1 and 2 are independently functional and testable.

---

## Phase 5: User Story 3 - Use Device-Specific Telemetry Values (Priority: P3)

**Goal**: Telemetry payload values come from a file named after the configured device, with clear failure handling.

**Independent Test**: Provide valid and invalid device-named profile files and verify telemetry starts only for valid profiles.

### Tests for User Story 3

- [X] T025 [P] [US3] Add test for loading telemetry values from the configured device-named profile file in simulator/tests/SmartHome.Simulator.Tests/telemetry/TelemetryProfileLoadingTests.cs
- [X] T026 [P] [US3] Add test for missing device-named profile file blocking telemetry start in simulator/tests/SmartHome.Simulator.Tests/telemetry/TelemetryProfileMissingFileTests.cs
- [X] T027 [P] [US3] Add test for malformed profile file blocking telemetry start with actionable error in simulator/tests/SmartHome.Simulator.Tests/telemetry/TelemetryProfileInvalidFileTests.cs

### Implementation for User Story 3

- [X] T028 [US3] Implement telemetry payload sequencing from loaded telemetry profile entries in simulator/src/SmartHome.Simulator/TelemetryPublisher.cs
- [X] T029 [US3] Integrate telemetry profile preflight validation into start-send-telemetry command flow in simulator/src/SmartHome.Simulator/Commands/TelemetryControlCommandHandlers.cs
- [X] T030 [US3] Add sample device-named telemetry profile documentation and format example in simulator/README.md

**Checkpoint**: All user stories are independently functional and testable.

---

## Phase 6: Polish & Cross-Cutting Concerns

**Purpose**: Final validation and cross-story quality improvements.

- [X] T031 [P] Align simulator runtime contract details with final command/config/profile behavior in specs/004-device-registration-simulator/contracts/simulator-runtime.yaml
- [X] T032 [P] Refresh quickstart validation steps with finalized command flow and expected outputs in specs/004-device-registration-simulator/quickstart.md
- [X] T033 Run simulator test suite and capture results in specs/004-device-registration-simulator/quickstart.md
- [X] T034 Run backend integration tests covering register/telemetry compatibility and capture results in specs/004-device-registration-simulator/quickstart.md
- [X] T035 Perform final README cleanup to remove legacy TODO section and present final command workflow in simulator/README.md

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies - can start immediately.
- **Foundational (Phase 2)**: Depends on Setup completion - blocks all user stories.
- **User Story Phases (3-5)**: Depend on Foundational completion.
- **Polish (Phase 6)**: Depends on all user stories being complete.

### User Story Dependencies

- **User Story 1 (P1)**: Starts after Phase 2; no dependency on US2/US3.
- **User Story 2 (P2)**: Starts after Phase 2 and benefits from US1 command-loop foundation in `Program.cs`.
- **User Story 3 (P3)**: Starts after Phase 2 and depends on US2 telemetry start workflow.

### Within Each User Story

- Write tests first and confirm they fail before implementation.
- Implement command handlers and runtime integration after model/state foundations.
- Update docs after behavior is validated.

### Parallel Opportunities

- Setup tasks marked [P] can run in parallel.
- Foundational tests T010 and T011 can run in parallel after core runtime files exist.
- US1 tests T012 and T013 can run in parallel.
- US2 tests T017, T018, and T019 can run in parallel.
- US3 tests T025, T026, and T027 can run in parallel.
- Polish documentation tasks T031 and T032 can run in parallel.

---

## Parallel Example: User Story 2

```bash
# Run US2 telemetry-control tests together:
Task: "Add test for start-send-telemetry command creating exactly one active send loop in simulator/tests/SmartHome.Simulator.Tests/command/StartTelemetryCommandTests.cs"
Task: "Add test for stop-send-telemetry when telemetry is active and when idle in simulator/tests/SmartHome.Simulator.Tests/command/StopTelemetryCommandTests.cs"
Task: "Add test for duplicate start-send-telemetry command feedback and no-op behavior in simulator/tests/SmartHome.Simulator.Tests/command/StartTelemetryDuplicateTests.cs"
```

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1: Setup.
2. Complete Phase 2: Foundational.
3. Complete Phase 3: User Story 1.
4. Validate registration-only flow before telemetry controls.

### Incremental Delivery

1. Deliver US1 command-loop + register-device path.
2. Add US2 telemetry start/stop controls with state protections.
3. Add US3 device-specific telemetry profile loading and validation.
4. Finish with Polish phase test execution and documentation updates.

### Parallel Team Strategy

1. One developer finalizes foundational runtime abstractions.
2. One developer focuses on command handlers and program dispatch.
3. One developer builds simulator tests and documentation updates in parallel.

---

## Notes

- [P] tasks use separate files or can proceed without waiting on unfinished same-file changes.
- [USx] labels map each task to a user story for independent delivery.
- All tasks include concrete file paths so execution is immediate.