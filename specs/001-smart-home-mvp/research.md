# Research: Smart Home MVP Telemetry Flow

## Decision 1: Architecture and runtime stack
- Decision: Use .NET 9 ASP.NET Core Web API, Angular frontend, SQL Server Express, and a .NET 9 simulator.
- Rationale: Matches project constraints, supports rapid MVP delivery, and aligns with constitutional architecture guidance.
- Alternatives considered: Node.js + React + PostgreSQL (rejected due to stack divergence), Blazor full-stack (rejected due to frontend choice already set to Angular).

## Decision 2: Device status evaluation
- Decision: Classify device as `active` when latest telemetry event time is within 5 minutes; otherwise `stale`. Devices with no telemetry are `stale`.
- Rationale: Clarified directly with stakeholder and supports deterministic operational behavior.
- Alternatives considered: Multi-state model (`active/warning/stale`) and configurable thresholds (deferred to post-MVP).

## Decision 3: Out-of-order telemetry handling
- Decision: Accept out-of-order telemetry and store it; compute latest status using event timestamp, not ingestion timestamp.
- Rationale: Prevents data loss and maintains historical integrity when network latency causes late arrival.
- Alternatives considered: Reject older events (rejected due to data loss risk), bounded lateness window (deferred complexity).

## Decision 4: Ingestion burst behavior
- Decision: Apply request validation and throttling safeguards; return structured `429` responses under temporary overload while preserving accepted data.
- Rationale: Provides predictable failure mode and protects API/database during short traffic spikes.
- Alternatives considered: Unlimited queueing in MVP (rejected due to operational risk), hard fail at web server level only (rejected due to poor client feedback).

## Decision 5: API contract style
- Decision: Define contracts in OpenAPI 3.0 with structured validation error payload.
- Rationale: Supports backend/frontend/simulator parallel work and contract testing.
- Alternatives considered: Ad hoc markdown-only endpoint docs (rejected due to weak machine readability).

## Decision 6: Observability minimums
- Decision: Emit structured logs for registration, ingestion, and latest-reading query paths with request correlation ids.
- Rationale: Required by constitution to diagnose ingestion and dashboard freshness issues.
- Alternatives considered: Debug-level local logging only (rejected due to low operational value).
