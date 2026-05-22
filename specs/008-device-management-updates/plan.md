# Implementation Plan: Device Management Updates

**Branch**: `008-device-management-updates` | **Date**: 2026-05-23 | **Spec**: [spec.md](spec.md)

**Input**: Feature specification from `specs/008-device-management-updates/spec.md`

## Summary

Deliver a device-management slice that lets operators inspect devices at a higher fidelity, register devices with the complete contract, unregister devices from the UI, and observe the latest telemetry value in list and details views. The implementation spans backend contract expansion, frontend list/detail flows, telemetry display, and operational logging with safer fetch handling.

## Technical Context

**Language/Version**: C# (.NET 9), TypeScript (Angular 19), JSON, Markdown

**Primary Dependencies**: ASP.NET Core Web API, Entity Framework Core, Serilog, Angular standalone components, Jasmine/Karma, SQL Server Express, existing simulator app

**Storage**: SQL Server Express via the existing SmartHome database and EF Core migrations when schema changes are required

**Testing**: xUnit integration tests for backend contracts and behavior, backend unit tests where needed, Angular component/service tests with Jasmine/Karma

**Target Platform**: Local development web application (backend API + Angular SPA + simulator)

**Project Type**: Web application (backend API + frontend SPA + simulator)

**Performance Goals**: Preserve current interactive performance for list/detail views; ensure telemetry and device updates remain responsive under normal local development load

**Constraints**: Keep explicit API contracts and HTTP methods as source of truth; use Serilog on .NET and console logging in Angular; preserve explicit CORS/origin configuration; avoid wildcard origins; fail safely on misconfiguration

**Scale/Scope**: Single backend API, one Angular feature area, and one simulator/runtime integration path

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- MVP Vertical Slice: PASS. The feature delivers an end-to-end slice where devices can be listed, inspected, registered, unregistered, and updated with latest telemetry through existing backend projections.
- API Contracts: PASS. Required changes are centered on explicit request/response contracts and HTTP verbs for list, register, unregister, and telemetry-driven projections.
- Test and Data Integrity: PASS. The plan includes backend integration tests, frontend unit tests, and migration/documentation work where needed.
- Observability: PASS. The plan defines Serilog-backed backend logging and Angular console logging for failures and operational diagnostics.
- Security and Configuration: PASS. The plan preserves explicit allowlists, safe defaults, and source-of-truth API contracts.

No constitution violations detected.

## Project Structure

### Documentation (this feature)

```text
specs/008-device-management-updates/
├── plan.md
├── research.md
├── data-model.md
├── quickstart.md
├── contracts/
│   └── device-management-api.md
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
    └── SmartHome.Api.IntegrationTests/

frontend/
├── src/
│   └── app/
│       └── features/
│           └── devices/
│               ├── device-details/
│               ├── device-list/
│               ├── device-registration/
│               ├── models/
│               └── services/
└── tests/

simulator/
└── src/
    └── SmartHome.Simulator/
```

**Structure Decision**: Use the existing web-application layout and extend the backend API contracts/endpoints, Angular devices feature area, integration tests, and simulator support without introducing new top-level applications.

## Phase 0: Research Output

Research decisions captured in [research.md](research.md):
- Keep the backend as the contract source of truth for device list, registration, and unregister operations.
- Treat latest telemetry as a projection derived from persisted telemetry readings rather than a separate write model.
- Add structured backend logging via Serilog and console logging in Angular for fetch, contract, and action failures.
- Preserve explicit CORS/origin controls and treat them as configuration, not code-path fallback logic.

## Phase 1: Design Output

- Data model: [data-model.md](data-model.md)
- Contract: [contracts/device-management-api.md](contracts/device-management-api.md)
- Validation runbook: [quickstart.md](quickstart.md)

Post-design constitution re-check: PASS across all gates.

## Phase 2: Implementation Preview

1. Extend backend contracts/endpoints for unregister and full device payload handling.
2. Update the backend device projection path so the latest telemetry value is always available in the list and details responses.
3. Add Serilog-based operational logging for the backend flows.
4. Update Angular device list/details/registration flows and ensure safe fetch handling plus console logging.
5. Add and run automated tests that cover contracts, telemetry projection, unregister behavior, and frontend error handling.

## Complexity Tracking

No justified complexity exceptions required.
