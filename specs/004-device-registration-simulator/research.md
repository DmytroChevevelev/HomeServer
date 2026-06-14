# Research: Simulator Device Registration Commands

## Decision 1: Reuse existing backend device and telemetry endpoints

- Decision: The simulator will call the existing `POST /api/devices` and `POST /api/telemetry` endpoints instead of introducing new backend routes.
- Rationale: The backend already exposes the required registration and ingestion contracts, including validation and duplicate-device handling, so adding new routes would duplicate logic and expand testing surface without new user value.
- Alternatives considered: Add simulator-specific backend endpoints for command-driven behavior; rejected because the simulator can orchestrate the flow client-side with the current API.

## Decision 2: Replace startup-only publishing with a stateful console command loop

- Decision: The simulator runtime will shift from immediate startup publishing to an interactive read-eval loop that accepts `register-device`, `start-send-telemetry`, and `stop-send-telemetry` commands.
- Rationale: The feature requires multiple commands in one session and explicit start/stop control, which the current single long-running `SendAsync` startup call cannot support safely.
- Alternatives considered: Keep automatic startup publishing with extra flags or restart-based control; rejected because it cannot satisfy the spec requirement for interactive command-driven control in one process.

## Decision 3: Model telemetry sending as a single-session state machine

- Decision: Telemetry runtime behavior will be guarded by explicit session state such as idle, sending, and faulted/stopped, with checks that prevent duplicate send loops.
- Rationale: The current infinite loop can only be cancelled by shutting down the process; a small state model is the simplest way to support safe start/stop semantics and operator feedback.
- Alternatives considered: Fire detached tasks without shared state; rejected because it makes duplicate loop prevention, stop semantics, and deterministic tests brittle.

## Decision 4: Store device telemetry inputs in a JSON file named after the configured device

- Decision: Each device will have a JSON telemetry values file whose base name matches the configured device identifier used by the simulator.
- Rationale: The simulator already uses JSON configuration, and a device-named file is a simple, deterministic lookup rule that satisfies the spec without requiring a central registry.
- Alternatives considered: Embed telemetry values in `appsettings.json`, generate random values, or use a single shared profile file; rejected because those options either reduce device specificity or make per-device test scenarios harder to reason about.

## Decision 5: Add a dedicated simulator test project

- Decision: The feature will introduce simulator-focused automated tests for command parsing, interval fallback, state transitions, and telemetry profile loading.
- Rationale: Most of the new behavior is local runtime logic inside the simulator, and the existing backend integration tests are not the right level to validate command REPL behavior or file parsing edge cases.
- Alternatives considered: Rely only on manual runs or expand backend integration tests to cover simulator internals; rejected because that would leave core simulator behavior weakly protected or force backend-hosted tests to cover client-only concerns.