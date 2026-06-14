# Implementation Plan: Simulator Device Registration Commands

**Branch**: `004-device-registration-simulator` | **Date**: 2026-05-22 | **Spec**: [spec.md](./spec.md)

**Input**: Feature specification from `/specs/004-device-registration-simulator/spec.md`

**Note**: This template is filled in by the `/speckit.plan` command. See `.specify/templates/plan-template.md` for the execution workflow.

## Summary

Add an interactive simulator command loop that can register a configured device, start telemetry on demand, and stop telemetry without exiting the process. The implementation will reuse the existing backend `POST /api/devices` and `POST /api/telemetry` contracts, move the simulator from a fire-and-forget startup flow to a stateful command-driven runtime, load telemetry values from a device-named file, and add automated tests around command state, configuration defaults, and backend request integration.

## Technical Context

**Language/Version**: C# on .NET 9

**Primary Dependencies**: `HttpClient` with `System.Net.Http.Json`, `Microsoft.Extensions.Configuration`, existing SmartHome backend request contracts, xUnit for automated tests

**Storage**: Backend SQL-backed API for registered devices and telemetry records, plus local JSON configuration and local device-specific telemetry values files in the simulator workspace

**Testing**: xUnit for simulator-focused tests plus targeted backend integration verification via `SmartHome.Api.IntegrationTests`

**Target Platform**: Local developer environments running the .NET simulator against the local backend API

**Project Type**: Console application with backend API integration

**Performance Goals**: Supported commands should produce operator feedback immediately after input, and active telemetry mode should honor the configured cadence with a default of one submission every 2 seconds

**Constraints**: Must keep one simulator process interactive across multiple commands; must prevent duplicate telemetry loops; must fail safely when the telemetry values file is missing or invalid; must not require new backend endpoints when existing contracts already satisfy the workflow

**Scale/Scope**: Single configured device per simulator process, three supported commands, one device-specific telemetry values file per device, and local development/demo usage

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- MVP Vertical Slice: PASS. The slice is simulator console input -> backend device registration/telemetry API -> persistence, demonstrated by registering one device and starting/stopping telemetry in a single session.
- API Contracts: PASS. The feature reuses existing backend registration and telemetry endpoints; the plan adds a simulator-facing contract artifact for console commands, configuration, and file expectations.
- Test and Data Integrity: PASS. The plan adds simulator automation for command/state logic and retains backend integration verification for registration and telemetry acceptance paths; no schema changes are required.
- Observability: PASS. Command execution results, state transitions, and telemetry file/configuration errors will be surfaced in simulator output.
- Security and Configuration: PASS. The plan keeps backend URL and interval configuration environment-safe, avoids secret logging, and fails closed on invalid telemetry profile inputs.

If any gate is not satisfied, document the gap and remediation in Complexity Tracking.

## Project Structure

### Documentation (this feature)

```text
specs/004-device-registration-simulator/
├── plan.md
├── research.md
├── data-model.md
├── quickstart.md
├── contracts/
│   └── simulator-runtime.yaml
└── tasks.md
```

### Source Code (repository root)

```text
simulator/
├── README.md
└── src/
    └── SmartHome.Simulator/
        ├── Program.cs
        ├── SmartHome.Simulator.csproj
        ├── TelemetryPublisher.cs
        ├── appsettings.json
        └── [new runtime support types for commands, state, and telemetry profile loading]

simulator/tests/
└── SmartHome.Simulator.Tests/
    ├── command/
    ├── configuration/
    └── telemetry/

backend/
├── src/
│   └── SmartHome.Api/
│       └── api/
│           ├── contracts/
│           └── endpoints/
└── tests/
    └── SmartHome.Api.IntegrationTests/
        ├── Devices/
        ├── Infrastructure/
        └── Telemetry/
```

**Structure Decision**: Keep implementation centered in `simulator/src/SmartHome.Simulator`, add a dedicated simulator test project for command and file-loading logic, and use existing backend integration tests only for end-to-end verification of the reused registration and telemetry routes.

## Phase 0: Research Findings

- Existing backend routes already satisfy the feature workflow: `POST /api/devices` registers devices and `POST /api/telemetry` accepts telemetry for registered devices.
- The current simulator starts telemetry immediately at process startup and has no interactive command model, no runtime state management, and no way to stop a send loop except process cancellation.
- Device-specific telemetry values are not modeled yet; a small local JSON file per device is the lowest-friction extension because the simulator already uses JSON-based configuration.
- A dedicated simulator test project is preferable to overloading backend integration tests because most new logic is local command parsing, state transitions, interval fallback handling, and telemetry profile validation.
- No backend schema or endpoint shape changes are required unless implementation uncovers a hard blocker in the existing registration request shape.

## Phase 1: Design Outputs

- `research.md`: Documents reuse of existing backend endpoints, command-loop design choice, telemetry profile file format choice, and simulator test strategy.
- `data-model.md`: Defines runtime entities for command input, simulator session state, device registration payload mapping, and telemetry profile records.
- `contracts/simulator-runtime.yaml`: Captures supported console commands, configuration keys, expected telemetry values file naming, and backend request payload dependencies.
- `quickstart.md`: Describes local validation by running backend + simulator, issuing the three commands, and confirming interval/default/file behaviors.

## Post-Design Re-Check

- MVP Vertical Slice: PASS. The design still delivers one interactive simulator flow against the live backend without depending on unfinished frontend work.
- API Contracts: PASS. Backend route reuse is explicit, and the simulator runtime contract defines the user-facing command/config/file interface.
- Test and Data Integrity: PASS. Simulator tests cover the new runtime logic, and backend integration checks remain focused on request acceptance and duplicate/unknown-device behavior.
- Observability: PASS. The design requires operator-visible output for command results, send-loop status, and file/configuration validation failures.
- Security and Configuration: PASS. Configuration stays local and explicit, with no secret material added and invalid setup prevented from silently sending telemetry.

## Complexity Tracking

No constitution violations identified.

## Implementation Validation Notes

- Validate simulator command behavior with automated tests for unsupported commands, duplicate start attempts, stop-before-start, and 2-second default interval fallback.
- Validate device-specific telemetry profile handling with tests for successful file load, missing file failure, and invalid file content failure.
- Validate end-to-end behavior by registering a device through the backend API, starting telemetry, and confirming stop command terminates further submissions without exiting the simulator.
