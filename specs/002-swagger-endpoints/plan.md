# Implementation Plan: Backend Swagger Endpoint Discovery

**Branch**: `002-swagger-endpoints` | **Date**: 2026-05-22 | **Spec**: [spec.md](./spec.md)

**Input**: Feature specification from `/specs/002-swagger-endpoints/spec.md`

**Note**: This template is filled in by the `/speckit.plan` command. See `.specify/templates/plan-template.md` for the execution workflow.

## Summary

Expose interactive API documentation and a machine-readable OpenAPI document for the backend so developers can discover and test endpoints quickly. The implementation approach is to add standard .NET Swagger/OpenAPI generation, gate documentation exposure by environment policy, preserve contract confidence with integration/contract checks, and add minimal diagnostics for docs availability and failures.

## Technical Context

**Language/Version**: C# on .NET 9 (ASP.NET Core minimal API)

**Primary Dependencies**: ASP.NET Core minimal APIs, Entity Framework Core 9 (existing), SQL Server provider (existing), Swagger/OpenAPI middleware and generator package (new)

**Storage**: SQL Server (existing); no new persistence entities or schema changes expected

**Testing**: xUnit unit and integration tests under `backend/tests`, with contract checks for documentation endpoints and OpenAPI document retrieval

**Target Platform**: Local development on Windows/macOS/Linux for backend service consumers (browser, frontend, simulator)

**Project Type**: Web application with backend API plus existing frontend and simulator clients

**Performance Goals**: Documentation UI and JSON document reachable within 2 seconds in local development and no material regression to existing API request latency

**Constraints**: Documentation exposure must be disabled outside allowed environments by default, existing endpoint contracts must remain stable, and no secrets may be exposed through docs examples/config

**Scale/Scope**: One backend service, current API surface (devices and telemetry endpoints), and local development usage by engineering/QA teams

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- MVP Vertical Slice: PASS. Run backend, open docs UI, inspect endpoint metadata, and execute a sample request from docs.
- API Contracts: PASS. Contract updates include documentation access behavior and machine-readable document retrieval expectations.
- Test and Data Integrity: PASS. Integration/contract test updates are planned; no data model migration is required.
- Observability: PASS. Plan includes diagnostics for docs endpoint requests and request-execution failures.
- Security and Configuration: PASS. Docs exposure is environment-gated with safe defaults and explicit unavailable behavior outside allowed environments.

If any gate is not satisfied, document the gap and remediation in Complexity Tracking.

### Post-Design Re-Check

- MVP Vertical Slice: PASS. Design artifacts preserve a runnable backend -> docs UI -> execute request flow.
- API Contracts: PASS. `contracts/openapi-docs.yaml` defines docs access expectations and unavailable behavior.
- Test and Data Integrity: PASS. Quickstart and plan define integration/contract validation and no schema migration.
- Observability: PASS. Research and plan require diagnostics for docs requests and failures.
- Security and Configuration: PASS. Docs access policy remains environment-gated with safe defaults.

## Project Structure

### Documentation (this feature)

```text
specs/002-swagger-endpoints/
├── plan.md              # This file (/speckit.plan command output)
├── research.md          # Phase 0 output (/speckit.plan command)
├── data-model.md        # Phase 1 output (/speckit.plan command)
├── quickstart.md        # Phase 1 output (/speckit.plan command)
├── contracts/           # Phase 1 output (/speckit.plan command)
└── tasks.md             # Phase 2 output (/speckit.tasks command - NOT created by /speckit.plan)
```

### Source Code (repository root)

```text
backend/
├── src/
│   └── SmartHome.Api/
│       ├── Program.cs
│       ├── api/
│       │   ├── contracts/
│       │   ├── endpoints/
│       │   ├── errors/
│       │   └── middleware/
│       ├── infrastructure/
│       └── services/
└── tests/
  ├── SmartHome.Api.UnitTests/
  └── SmartHome.Api.IntegrationTests/

frontend/
├── src/
└── tests/

simulator/
└── src/
  └── SmartHome.Simulator/

specs/
└── 002-swagger-endpoints/
  ├── spec.md
  ├── plan.md
  ├── research.md
  ├── data-model.md
  ├── quickstart.md
  └── contracts/
```

**Structure Decision**: Use the existing web application structure and implement documentation behavior in `backend/src/SmartHome.Api` with validation in integration/contract tests under `backend/tests`.

## Complexity Tracking

No constitution violations identified.

## Observability Notes

- Documentation path requests (`/swagger*`) are logged with request path details.
- Failed documentation requests (HTTP >= 400) emit warning logs with status and path.
- Existing request correlation middleware continues to stamp and log all docs and API requests.
