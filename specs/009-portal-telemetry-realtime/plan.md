# Implementation Plan: Portal Telemetry Realtime

**Branch**: `009-portal-telemetry-realtime` | **Date**: 2026-05-23 | **Spec**: [spec.md](./spec.md)

**Input**: Feature specification from `/specs/009-portal-telemetry-realtime/spec.md`

## Summary

Implement real-time sensor updates in the portal by introducing backend-to-frontend notification on telemetry ingestion and adding a selected-device telemetry-list endpoint. Update the frontend device list to react to backend notifications for Latest Value updates and replace the device-details telemetry section with a contract-aligned telemetry list.

## Technical Context

**Language/Version**: C# (.NET 9), TypeScript (Angular 19), Markdown

**Primary Dependencies**: ASP.NET Core Minimal APIs, ASP.NET Core SignalR, EF Core, existing repositories/services, Angular standalone components/services, Jasmine/Karma

**Storage**: SQL Server Express via existing SmartHome database and TelemetryReadings table

**Testing**: xUnit (backend integration/unit), Jasmine/Karma (frontend unit/component)

**Target Platform**: Local web deployment (SmartHome API + Angular SPA + simulator telemetry ingress)

**Project Type**: Web application (backend API + frontend SPA)

**Performance Goals**: Latest Value column updates visible in-session after ingest notification; selected-device telemetry list loads in under 3 seconds for normal local test datasets

**Constraints**: Preserve existing API contracts and HTTP methods as source of truth, keep CORS allowlist explicit, provide OpenAPI metadata for modified endpoints, keep logging actionable for notification and telemetry retrieval failures

**Scale/Scope**: Single feature slice across backend telemetry paths and frontend devices pages; no new top-level applications

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- MVP Vertical Slice: PASS. Users can see latest values update in device list and inspect selected-device telemetry list from details in one end-to-end flow.
- API Contracts: PASS. New GET `/api/devices/{deviceId}/telemetry` contract and notification payload are defined with validation/error expectations.
- Test and Data Integrity: PASS. Plan includes backend endpoint/notification tests and frontend notification/list rendering tests; no schema change required.
- Observability: PASS. Plan requires logging for notification publish, hub connection lifecycle, and telemetry retrieval failures.
- Security and Configuration: PASS. Uses existing auth/CORS/configuration patterns; no secrets added; failures return safe user-facing errors.

No constitution violations detected.

## Project Structure

### Documentation (this feature)

```text
specs/009-portal-telemetry-realtime/
├── plan.md
├── research.md
├── data-model.md
├── quickstart.md
├── contracts/
│   └── portal-telemetry-api.md
└── tasks.md
```

### Source Code (repository root)

```text
backend/
├── src/
│   └── SmartHome.Api/
│       ├── api/
│       │   ├── contracts/
│       │   ├── endpoints/
│       │   └── middleware/
│       ├── infrastructure/
│       ├── models/
│       └── services/
└── tests/
    ├── SmartHome.Api.UnitTests/
    └── SmartHome.Api.IntegrationTests/

frontend/
├── src/
│   └── app/
│       └── features/
│           └── devices/
│               ├── device-list/
│               ├── device-details/
│               ├── models/
│               └── services/
└── tests/
```

**Structure Decision**: Extend existing backend endpoints/services and frontend devices feature modules. Add no new top-level projects.

## Phase 0: Research Output

Research captured in [research.md](./research.md):
- SignalR chosen for backend-to-frontend notification when telemetry is ingested.
- New device-scoped telemetry list endpoint selected over frontend filtering of all-device projections.
- Contract-driven DTO mapping preserved to avoid latest-value field drift.
- Observability requirements defined for publish/reconnect/retrieval failures.

## Phase 1: Design Output

- Data model: [data-model.md](./data-model.md)
- Contracts: [contracts/portal-telemetry-api.md](./contracts/portal-telemetry-api.md)
- Validation runbook: [quickstart.md](./quickstart.md)

Post-design constitution re-check: PASS across all gates.

## Phase 2: Implementation Preview

1. Backend notification pipeline
   - Add SignalR hub route for telemetry notifications.
   - Publish `sensorValueChanged` event after successful telemetry persistence.
   - Add logging for publish success/failure and correlation context.
2. Backend telemetry list endpoint
   - Add `GET /api/devices/{deviceId}/telemetry` with optional date-range/limit filters.
   - Return selected-device telemetry rows sorted by `eventTimeUtc` descending.
   - Add OpenAPI summary, parameter docs, and response docs on endpoint.
3. Frontend list real-time update
   - Add notification client service and subscribe in device-list flow.
   - Update Latest Value column on notification (patch or re-fetch by device).
   - Preserve backend contract field names (`latestMetricValue`, `latestEventTimeUtc`).
4. Frontend details telemetry list
   - Replace generic telemetry section with list rendering bound to telemetry list contract.
   - Load list from new device-scoped endpoint, including empty and error states.
5. Testing and validation
   - Backend integration tests for new endpoint filtering/order and notification publish trigger.
   - Frontend unit/component tests for real-time list updates and details list contract rendering.
   - End-to-end local validation using simulator telemetry ingestion.

## Complexity Tracking

No justified complexity exceptions required.
