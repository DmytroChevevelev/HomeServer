# Feature Specification: Simulator Device Registration Commands

**Feature Branch**: `004-device-registration-simulator`

**Created**: 2026-05-22

**Status**: Draft

**Input**: User description: "implement device registration behavior. console gets command from input and send request to the backend. there are three command: register-device, start-send-telemetry and stop-send-telemetry. configuration should allow to configure an interval between telemetry data. By default - 2sec. values for telemetry should come from additional file with the same name as device name. those changes related to simulator"

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Register A Simulator Device (Priority: P1)

As a developer running the simulator, I can issue a `register-device` command so the configured device is created or confirmed in the backend before telemetry starts.

**Why this priority**: Device registration is the gating action that makes the simulator usable in fresh local environments.

**Independent Test**: Can be fully tested by starting the simulator, entering `register-device`, and verifying the backend accepts the device and the simulator reports the result.

**Acceptance Scenarios**:

1. **Given** the simulator is running with a valid backend address and device configuration, **When** the operator enters `register-device`, **Then** the simulator sends a registration request for the configured device and reports whether the backend accepted it.
2. **Given** the backend rejects registration or is unavailable, **When** the operator enters `register-device`, **Then** the simulator reports a clear failure outcome and remains available for retry.

---

### User Story 2 - Control Telemetry From The Console (Priority: P2)

As a developer running a demo or test, I can start and stop telemetry by entering console commands so I can control when the simulator sends data without restarting it.

**Why this priority**: Interactive telemetry control is the core day-to-day behavior the operator needs after registration succeeds.

**Independent Test**: Can be fully tested by entering `start-send-telemetry`, observing repeated telemetry submissions, then entering `stop-send-telemetry` and confirming submissions stop while the simulator stays interactive.

**Acceptance Scenarios**:

1. **Given** the simulator is idle, **When** the operator enters `start-send-telemetry`, **Then** the simulator begins sending telemetry for the configured device at the active interval.
2. **Given** telemetry is actively being sent, **When** the operator enters `stop-send-telemetry`, **Then** the simulator stops sending new telemetry and continues waiting for further commands.

---

### User Story 3 - Use Device-Specific Telemetry Values (Priority: P3)

As a developer testing multiple devices, I can provide telemetry values in a file named after the device so each simulator run uses the correct device-specific data set.

**Why this priority**: Device-specific telemetry makes simulator runs realistic and prevents every device from emitting the same generic data.

**Independent Test**: Can be fully tested by configuring a device name, providing a matching telemetry file, starting telemetry, and verifying the emitted values come from that file.

**Acceptance Scenarios**:

1. **Given** a configured device with a matching telemetry values file, **When** telemetry starts, **Then** the simulator loads values from that device-specific file before sending readings.
2. **Given** the telemetry values file is missing or invalid, **When** the operator starts telemetry, **Then** the simulator reports the file problem and does not silently send unintended values.

### Edge Cases

- The operator enters an unsupported command; the simulator shows the list of supported commands and remains interactive.
- The operator enters `start-send-telemetry` while telemetry is already running; the simulator avoids creating duplicate send loops.
- The operator enters `stop-send-telemetry` before telemetry has started; the simulator reports that no active send loop exists.
- The telemetry interval is missing, zero, or invalid in configuration; the simulator falls back to the default interval instead of failing unpredictably.
- The configured device name does not match an available telemetry values file; the simulator surfaces an actionable error before sending data.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: System MUST accept console input while running and recognize the commands `register-device`, `start-send-telemetry`, and `stop-send-telemetry`.
- **FR-002**: System MUST send a backend device registration request for the configured device when the operator enters `register-device`.
- **FR-003**: System MUST provide clear command feedback that indicates whether each command succeeded, failed, or could not run due to current simulator state.
- **FR-004**: System MUST begin recurring telemetry submission for the configured device when the operator enters `start-send-telemetry` and prerequisites are satisfied.
- **FR-005**: System MUST stop recurring telemetry submission when the operator enters `stop-send-telemetry` without requiring the simulator process to exit.
- **FR-006**: System MUST allow telemetry cadence to be configured and MUST default to a 2-second interval when no valid override is provided.
- **FR-007**: System MUST load telemetry values from an external file whose name matches the configured device name before sending telemetry for that device.
- **FR-008**: System MUST prevent telemetry from starting when the device-specific values file cannot be found or parsed successfully.
- **FR-009**: System MUST prevent concurrent duplicate telemetry send loops for the same simulator process.
- **FR-010**: Users MUST be able to issue multiple supported commands in one simulator session without restarting the simulator.

### Key Entities *(include if feature involves data)*

- **Simulator Command**: A user-entered console instruction that requests device registration or changes telemetry sending state.
- **Simulator Device Profile**: The configured device identity and runtime settings needed to address backend requests and locate the device-specific telemetry values file.
- **Telemetry Values File**: An external data source named after the device that defines the values the simulator should emit during telemetry sending.
- **Telemetry Session State**: The current runtime status that indicates whether the simulator is idle, actively sending telemetry, or blocked by a validation or connectivity error.

## Constitution Alignment *(mandatory)*

- **MVP Slice**: Start the simulator, register one configured device, start telemetry from the console, then stop telemetry without restarting the process.
- **API Contracts**: No new public API routes are introduced by this feature; the simulator must call the existing backend registration and telemetry routes and surface request validation, connectivity, and failure outcomes clearly to the operator.
- **Testing Scope**: Minimum coverage includes command parsing and state transitions, telemetry interval default behavior, device-specific file loading, and end-to-end verification that registration and telemetry requests are sent only in the correct runtime states.
- **Observability**: Command handling, state changes, registration outcomes, telemetry start and stop events, and device profile file errors must be visible in normal simulator output for debugging.
- **Security/Configuration**: Simulator configuration must allow safe backend address and interval settings without embedding secrets in logs, and invalid configuration must fail safely with operator-readable guidance.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: An operator can launch the simulator, register the configured device, and begin telemetry from console commands alone in under 30 seconds in a standard local setup.
- **SC-002**: When no valid interval override is configured, telemetry is sent at the default cadence of one submission every 2 seconds during active send mode.
- **SC-003**: 100% of supported commands return clear feedback about success, failure, or invalid state in validation scenarios for this feature.
- **SC-004**: In all validation scenarios with a matching device-specific values file, the simulator loads that file before the first telemetry submission for the session.

## Assumptions

- The backend already exposes the device registration and telemetry ingestion capabilities needed by the simulator.
- Each simulator process works with one configured device at a time.
- Device name and device-specific telemetry file name use the same identifier so the simulator can resolve the correct file deterministically.
- The feature scope is limited to simulator behavior and any minimal backend interaction changes required to support that simulator workflow.