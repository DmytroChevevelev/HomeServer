# Data Model: Simulator Device Registration Commands

## SimulatorCommand

- Purpose: Represents one operator-entered instruction processed by the simulator runtime.
- Fields:
  - `Name`: normalized command token entered at the console.
  - `Arguments`: optional additional tokens or values following the command.
  - `ReceivedAtUtc`: timestamp used for diagnostics and sequencing.
- Validation:
  - `Name` must match one of `register-device`, `start-send-telemetry`, or `stop-send-telemetry` for a supported command path.
  - Unsupported commands must not mutate runtime state.

## SimulatorDeviceProfile

- Purpose: Defines the configured device identity and runtime configuration needed for backend requests and telemetry profile resolution.
- Fields:
  - `ApiBaseUrl`: base backend API URL.
  - `DeviceExternalId`: external device identifier used for registration and telemetry ingestion.
  - `DeviceName`: human-readable device name sent in registration requests.
  - `SensorType`: sensor classification sent in registration requests.
  - `SendIntervalSeconds`: configured telemetry interval, defaulting to `2` when absent or invalid.
  - `TelemetryProfilePath`: resolved path to the device-specific telemetry values file.
- Validation:
  - `ApiBaseUrl` must be non-empty and parseable as an absolute HTTP or HTTPS base URL.
  - `DeviceExternalId`, `DeviceName`, and `SensorType` must be present before device registration can run.
  - `SendIntervalSeconds` must resolve to a positive value; otherwise the runtime uses the default.

## TelemetryProfile

- Purpose: Holds the device-specific telemetry values loaded from the external file before telemetry starts.
- Fields:
  - `Metrics`: ordered list of telemetry metric entries to emit.
  - `SourceDeviceName`: device identifier inferred from the file naming convention.
  - `LoadedAtUtc`: timestamp of successful load for diagnostics.
- Validation:
  - The file name must match the configured device identifier used for profile lookup.
  - `Metrics` must contain at least one valid entry before telemetry can start.
  - Invalid or unreadable content must block telemetry startup with a clear error.

## TelemetryMetricEntry

- Purpose: Represents one metric sample the simulator can submit during an active send loop.
- Fields:
  - `MetricType`: metric label such as temperature or humidity.
  - `MetricValue`: numeric value sent to the backend.
  - `EventTimeStrategy`: indicates whether the runtime uses current UTC time or a file-provided event time.
- Validation:
  - `MetricType` must be non-empty.
  - `MetricValue` must be parseable as a numeric value.

## TelemetrySessionState

- Purpose: Tracks whether telemetry is idle, actively sending, stopped, or blocked by an execution error.
- States:
  - `Idle`: no active telemetry send loop exists.
  - `Sending`: one active telemetry send loop is running.
  - `Faulted`: the last attempted start or active send failed and requires operator attention.
- Transitions:
  - `Idle -> Sending` on successful `start-send-telemetry` after profile validation.
  - `Sending -> Idle` on `stop-send-telemetry`.
  - `Idle -> Idle` on successful `register-device`.
  - `Idle -> Faulted` or `Sending -> Faulted` on unrecoverable file or request failures that stop safe execution.