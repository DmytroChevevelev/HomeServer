# Research: Device Management Updates

## Decision 1: Keep the backend as the source of truth for device contracts
- Decision: Treat backend request/response shapes and HTTP verbs as the authoritative contract for list, registration, unregister, and telemetry projection flows.
- Rationale: The frontend and simulator already depend on backend payload shape; keeping one source of truth prevents mapping drift.
- Alternatives considered: duplicating contract definitions in the frontend only, rejected because it increases drift risk and hides backend changes.

## Decision 2: Model latest telemetry as a projection over persisted readings
- Decision: Continue to compute the latest telemetry value from stored telemetry readings instead of storing a separate latest-value write model for this feature.
- Rationale: The existing backend already projects latest telemetry from readings, which keeps telemetry history and visible summary aligned.
- Alternatives considered: maintaining a separate summary table or cache, rejected for this increment because it adds write-path complexity without improving the MVP slice.

## Decision 3: Use Serilog for backend operational logging
- Decision: Emit structured backend logs through Serilog for registration, unregister, telemetry ingestion, and device projection failures.
- Rationale: Structured logs make it easier to diagnose contract mismatches, projection failures, and API issues across the backend workflow.
- Alternatives considered: relying on default console logging only, rejected because the feature explicitly requires stronger backend observability.

## Decision 4: Use console logging in Angular for request-flow diagnostics
- Decision: Log frontend request and action failures to the browser console with operation context and API base URL.
- Rationale: The frontend needs a lightweight diagnostic path that helps distinguish fetch/context issues, contract mismatches, and backend failures during local development.
- Alternatives considered: adding a custom logging service or external telemetry provider, rejected because the feature scope only requires local operational diagnostics.

## Decision 5: Preserve explicit CORS/origin controls
- Decision: Keep explicit origin allowlists as configuration and do not introduce wildcard fallbacks.
- Rationale: The repo has already demonstrated origin-specific browser behavior; explicit controls keep local development safe and predictable.
- Alternatives considered: wildcard CORS or dynamic reflection of the `Origin` header, rejected because they weaken security posture and obscure configuration mistakes.

## Decision 6: Add regression coverage for contract and fetch-binding failures
- Decision: Add tests that validate contract compatibility, list/detail mapping, unregister behavior, and context-safe fetch usage.
- Rationale: The repo has already seen failures caused by stale field names and `fetch` binding issues; tests should lock those cases down.
- Alternatives considered: manual browser checks only, rejected because the observed failures were subtle and easy to miss.