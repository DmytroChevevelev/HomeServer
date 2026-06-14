# Implementation Plan: Portal Telemetry Realtime

**Branch**: `009-portal-telemetry-realtime` | **Date**: 2026-05-23 | **Spec**: [spec.md](./spec.md)

**Input**: Feature specification from `/specs/009-portal-telemetry-realtime/spec.md`

## Summary

Deliver a real-time telemetry slice where the portal device list reflects live sensor-value changes and the device details page shows selected-device telemetry records from a dedicated endpoint. The design includes SignalR notifications, a latest-100 default telemetry-history API with configurable limits, and unregister behavior that removes related telemetry data.

## Technical Context

**Language/Version**: C# (.NET 9), TypeScript (Angular 19), Markdown

**Primary Dependencies**: ASP.NET Core Minimal APIs, ASP.NET Core SignalR, Entity Framework Core, Serilog, Angular standalone components/services, Jasmine/Karma

**Storage**: SQL Server Express (SmartHome database, Devices and TelemetryReadings tables)

**Testing**: xUnit (backend integration/unit), Jasmine/Karma (frontend unit/component)

**Target Platform**: Local web app stack (ASP.NET Core API + Angular SPA + simulator telemetry ingestion)

**Project Type**: Web application (backend + frontend)

**Performance Goals**: Device list latest value updates in-session after telemetry ingest; selected-device telemetry list returns newest-first records with default limit 100

**Constraints**: Preserve API contracts and HTTP method intent; include OpenAPI metadata on changed endpoints; keep routing composition explicit (`/api` vs `/hubs`); ensure telemetry cleanup on device unregister

**Scale/Scope**: One backend API and one frontend devices feature module; no additional applications or infrastructure tracks

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- MVP Vertical Slice: PASS. End-to-end flow includes telemetry ingest -> realtime notification -> list update and device-details telemetry history retrieval.
- API Contracts: PASS. New selected-device telemetry endpoint is explicitly contracted with validation/error responses and OpenAPI metadata.
- Test and Data Integrity: PASS. Plan includes backend endpoint and notification tests, frontend rendering/realtime tests, and unregister cleanup validation.
- Observability: PASS. Logging requirements cover telemetry ingestion, realtime publish outcomes, and selected-device history retrieval.
- Security and Configuration: PASS. Existing CORS/origin allowlist remains explicit, no secrets are added, and misconfiguration should fail safely.

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
│       │   └── hubs/
│       ├── infrastructure/
│       │   └── repositories/
│       └── services/
└── tests/
    ├── SmartHome.Api.IntegrationTests/
    └── SmartHome.Api.UnitTests/

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

**Structure Decision**: Extend existing backend endpoint/repository/service layers and frontend devices feature modules; keep the current project topology unchanged.

## Phase 0: Research Output

Research captured in [research.md](./research.md):
- SignalR selected for backend-to-frontend sensor update notifications.
- Device-scoped telemetry endpoint selected over frontend filtering.
- Contract-driven field mapping retained to avoid DTO drift.
- Observability requirements added for realtime and retrieval paths.

## Phase 1: Design Output

- Data model: [data-model.md](./data-model.md)
- Contracts: [contracts/portal-telemetry-api.md](./contracts/portal-telemetry-api.md)
- Validation runbook: [quickstart.md](./quickstart.md)

Post-design constitution re-check: PASS across all gates.

## Phase 2: Implementation Preview

1. Realtime list updates
   - Publish `sensorValueChanged` via SignalR after telemetry ingest persistence.
   - Subscribe in device list and patch matching row latest value/time fields.
2. Selected-device telemetry endpoint
   - Add `GET /api/devices/{deviceId}/telemetry` with default `limit=100` and caller-specified limit support.
   - Return newest-first telemetry rows for the selected device.
3. Device details telemetry list UI
   - Replace list source with selected-device endpoint.
   - Add user control to specify returned telemetry count.
4. Unregister data cleanup
   - Ensure device unregister removes all related telemetry records to avoid dead data.
5. Validation and tests
   - Add/update backend integration tests for endpoint limit behavior and unregister cleanup.
   - Add/update frontend tests for details list rendering and selectable limits.

## Complexity Tracking

No justified complexity exceptions required.
