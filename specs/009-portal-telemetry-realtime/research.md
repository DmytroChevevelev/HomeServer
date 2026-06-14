# Phase 0 Research: Portal Telemetry Realtime

## Decision 1: Use SignalR to notify frontend of sensor value changes
- Decision: Add an ASP.NET Core SignalR hub (for example, /hubs/telemetry) that pushes a sensor-value-changed event whenever telemetry ingestion succeeds.
- Rationale: SignalR is first-class in .NET and Angular/browser clients, supports automatic reconnect, and fits server-to-client push without polling loops.
- Alternatives considered:
  - Server-Sent Events (SSE): Simpler but less flexible for future bidirectional use and weaker built-in reconnection ergonomics compared to SignalR client features.
  - Frontend polling of /api/devices: Easy to implement but does not satisfy strong real-time behavior and increases request load.

## Decision 2: Introduce a device-scoped telemetry history endpoint
- Decision: Add GET /api/devices/{deviceId}/telemetry to return telemetry list for a selected device, ordered by EventTimeUtc descending.
- Rationale: The details page needs selected-device history and should not fetch all-device latest projection then filter client-side.
- Alternatives considered:
  - Reuse GET /api/telemetry/latest and filter in frontend: Incorrect shape for history and can miss records.
  - Add query-based endpoint under /api/telemetry: Viable, but device-scoped route under devices is clearer for selected-device UI behavior.

## Decision 3: Keep list and details contracts aligned to backend DTOs
- Decision: Keep latest-value mapping contract-driven and use telemetry history DTO that matches backend response fields explicitly.
- Rationale: Repository history already shows breakage when frontend expected aliases that differed from backend field names.
- Alternatives considered:
  - Frontend-only alias expansion without contract tests: Faster short-term but risk of silent drift and regressions.

## Decision 4: Update UI flow to react to push events and refresh list projection
- Decision: On telemetry notification, refresh or patch only affected device row so Latest Value column updates in-session.
- Rationale: Guarantees visible real-time behavior while preserving backend as source of truth for projection logic.
- Alternatives considered:
  - Local optimistic updates from event payload only: Lower latency but risks divergence from server-calculated status/fields.

## Decision 5: Preserve observability and error diagnostics
- Decision: Add logs for telemetry notification publishing, hub connection lifecycle, and device-scoped telemetry retrieval failures.
- Rationale: Real-time flows are harder to debug; explicit logs are required by constitution observability gate.
- Alternatives considered:
  - Minimal logging: Lower noise but insufficient to diagnose stale UI update paths.
