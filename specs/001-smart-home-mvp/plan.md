# Implementation Plan: Smart Home MVP Telemetry Flow

**Branch**: `001-new-specification` | **Date**: 2026-05-22 | **Spec**: [spec.md](./spec.md)

**Input**: Feature specification from `/specs/001-smart-home-mvp/spec.md`

**Note**: This template is filled in by the `/speckit.plan` command. See `.specify/templates/plan-template.md` for the execution workflow.

## Summary

Deliver an MVP vertical slice that allows operators to register devices, ingest telemetry,
persist records in SQL Server Express, and view latest telemetry/status in Angular.
Implementation will follow contract-driven API design, EF Core migration-based schema evolution,
and simulator-assisted end-to-end validation.

## Technical Context

<!--
  ACTION REQUIRED: Replace the content in this section with the technical details
  for the project. The structure here is presented in advisory capacity to guide
  the iteration process.
-->

**Language/Version**: C# on .NET 9 (backend + simulator), TypeScript (Angular frontend)

**Primary Dependencies**: ASP.NET Core Web API, Entity Framework Core, Angular, SQL Server Express provider

**Storage**: Microsoft SQL Server Express

**Testing**: xUnit (backend unit/integration), Angular test runner for UI component/service checks, contract tests for API schemas

**Target Platform**: Local development on Windows/macOS/Linux

**Project Type**: Web application (backend API + frontend SPA + simulator)

**Performance Goals**: 95% of valid telemetry events visible in dashboard within 15 seconds; support 500 telemetry events in 10 minutes without data loss in local validation

**Constraints**: Active/stale threshold fixed at 5 minutes; never-reported devices are stale; single-tenant MVP; no secrets in source; CORS must explicitly allow frontend origin

**Scale/Scope**: Local MVP with core flows (device registration, telemetry ingest, latest telemetry dashboard)

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- MVP Vertical Slice: PASS. Plan includes register -> ingest -> persist -> dashboard flow as first increment.
- API Contracts: PASS. Device and telemetry contracts are defined in `/contracts/openapi.yaml`.
- Test and Data Integrity: PASS. xUnit integration tests and EF Core migrations are included in scope.
- Observability: PASS. Logging and basic health diagnostics are required for registration, ingestion, and query paths.
- Security and Configuration: PASS. Environment-configured URLs, SQL connection string, and CORS origin controls are specified.

## Project Structure

### Documentation (this feature)

```text
specs/[###-feature]/
├── plan.md              # This file (/speckit.plan command output)
├── research.md          # Phase 0 output (/speckit.plan command)
├── data-model.md        # Phase 1 output (/speckit.plan command)
├── quickstart.md        # Phase 1 output (/speckit.plan command)
├── contracts/           # Phase 1 output (/speckit.plan command)
└── tasks.md             # Phase 2 output (/speckit.tasks command - NOT created by /speckit.plan)
```

### Source Code (repository root)
<!--
  ACTION REQUIRED: Replace the placeholder tree below with the concrete layout
  for this feature. Delete unused options and expand the chosen structure with
  real paths (e.g., apps/admin, packages/something). The delivered plan must
  not include Option labels.
-->

```text
backend/
├── src/
│   ├── api/
│   ├── models/
│   ├── services/
│   └── infrastructure/
└── tests/
  ├── integration/
  └── unit/

frontend/
├── src/
│   ├── app/
│   │   ├── features/
│   │   ├── core/
│   │   └── shared/
│   └── environments/
└── tests/

simulator/
└── src/
```

**Structure Decision**: Web application structure is selected to preserve clean separation between API, Angular UI, and simulator while enabling independent tests.

## Post-Design Constitution Re-Check

- MVP Vertical Slice: PASS. Data model, contracts, and quickstart preserve a single demonstrable flow.
- API Contracts: PASS. `contracts/openapi.yaml` defines endpoint payloads and structured errors.
- Test and Data Integrity: PASS. Plan and quickstart include integration + contract testing and migration execution.
- Observability: PASS. Research and spec include required diagnostics coverage for core flows.
- Security and Configuration: PASS. Quickstart and plan require environment-based configuration and explicit CORS.

## Complexity Tracking

> **Fill ONLY if Constitution Check has violations that must be justified**

| Violation | Why Needed | Simpler Alternative Rejected Because |
|-----------|------------|-------------------------------------|
| None | N/A | N/A |
