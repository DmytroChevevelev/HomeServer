# Implementation Plan: Database Sync and API Docs Metadata

**Branch**: `003-update-db-docs` | **Date**: 2026-05-22 | **Spec**: [spec.md](./spec.md)

**Input**: Feature specification from `/specs/003-update-db-docs/spec.md`

**Note**: This template is filled in by the `/speckit.plan` command. See `.specify/templates/plan-template.md` for the execution workflow.

## Summary

Deliver a reliable database creation/update flow and tighten OpenAPI documentation quality for modified routes. The approach is to use existing EF Core migrations for schema synchronization, preserve data integrity during upgrades, and enforce documentation consistency by aligning endpoint summaries/parameter descriptions/response descriptions with contract artifacts and integration checks.

## Technical Context

**Language/Version**: C# on .NET 9 (ASP.NET Core minimal API)

**Primary Dependencies**: EF Core 9 with SQL Server provider, ASP.NET Core OpenAPI/Swagger generation, existing endpoint metadata APIs

**Storage**: SQL Server database (`SmartHomePortalDb`) with EF Core migrations

**Testing**: xUnit unit/integration tests (`SmartHome.Api.UnitTests`, `SmartHome.Api.IntegrationTests`) with contract checks on Swagger JSON

**Target Platform**: Local development environments (Windows/macOS/Linux) and CI integration test execution for backend service

**Project Type**: Web application backend API with supporting frontend and simulator clients

**Performance Goals**: Database synchronization command completes successfully for local dev environments without manual SQL intervention; docs endpoints remain reachable in development within existing baseline behavior

**Constraints**: Must use migration-driven schema updates only; maintain docs environment-gating policy; endpoint documentation updates must align with `specs/002-swagger-endpoints/contracts/openapi-docs.yaml`

**Scale/Scope**: Single backend service, existing device/telemetry routes, migration-application validation (and new migration creation only when model changes require it), and contract/doc coverage for modified endpoints

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- MVP Vertical Slice: PASS. Run migration-driven DB sync, start API, and verify one updated endpoint documentation entry in Swagger UI/JSON.
- API Contracts: PASS. Documentation routes are constrained by the existing contract source of truth and feature contracts add endpoint metadata expectations for modified routes.
- Test and Data Integrity: PASS. Plan includes migration application checks and integration/contract test updates for docs metadata coverage.
- Observability: PASS. Existing docs request logging middleware flow is retained and validation/test failures provide diagnostics for documentation drift.
- Security and Configuration: PASS. Docs remain disabled in non-development by default, and DB connection settings remain environment-driven.

If any gate is not satisfied, document the gap and remediation in Complexity Tracking.

## Project Structure

### Documentation (this feature)

```text
specs/003-update-db-docs/
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
│       │   └── middleware/
│       ├── infrastructure/
│       │   ├── SmartHomeDbContext.cs
│       │   └── migrations/
│       └── services/
└── tests/
  ├── SmartHome.Api.UnitTests/
  └── SmartHome.Api.IntegrationTests/

specs/
└── 003-update-db-docs/
  ├── spec.md
  ├── plan.md
  ├── research.md
  ├── data-model.md
  ├── quickstart.md
  └── contracts/

docs/
└── development-commands.md

```

**Structure Decision**: Use the existing backend web API and integration-test layout. Implement schema-sync and documentation-alignment changes in `backend/src/SmartHome.Api` and validate with tests in `backend/tests/SmartHome.Api.IntegrationTests`.

## Phase 0: Research Findings

- Database synchronization SHOULD use EF Core migration commands (`dotnet ef database update`) as the canonical create/update mechanism for both fresh and existing environments.
- New EF Core migration files SHOULD be created only when model or mapping changes require schema evolution; otherwise the workflow applies existing migrations.
- Endpoint documentation quality SHOULD be enforced through OpenAPI metadata in endpoint mappings (`WithSummary`, parameter docs where applicable, and explicit `Produces` response docs).
- Documentation route behavior MUST stay consistent with `specs/002-swagger-endpoints/contracts/openapi-docs.yaml` as the source-of-truth contract for `/swagger`, `/swagger/index.html`, and `/swagger/v1/swagger.json`.
- Contract/integration tests SHOULD validate both availability policy and metadata completeness to prevent drift.

## Phase 1: Design Outputs

- `research.md`: Decisions and alternatives for migration strategy, docs metadata alignment, and validation approach.
- `data-model.md`: Defines schema state and documentation metadata entities required for this feature.
- `contracts/openapi-docs-alignment.yaml`: Extends documentation contract expectations to include required metadata for modified API operations.
- `quickstart.md`: End-to-end verification steps for database creation/update and docs metadata checks.

## Post-Design Re-Check

- MVP Vertical Slice: PASS. Flow covers migration -> API startup -> documentation verification.
- API Contracts: PASS. Contracts include docs route policy plus metadata expectations for changed endpoints.
- Test and Data Integrity: PASS. Plan requires migration validation and automated contract checks.
- Observability: PASS. Existing docs request/failure logs and integration test diagnostics remain available.
- Security and Configuration: PASS. Environment-based docs policy and connection string configuration are preserved.

## Complexity Tracking

No constitution violations identified.

## Implementation Validation Notes

- Migration diagnostics now log startup schema state without auto-applying migrations.
- Configuration flags `Database:LogMigrationStateOnStartup`, `Database:ApplyMigrationsOnStartup`, and `Docs:RequireOpenApiMetadataDescriptions` are documented and active in development settings.
- Integration tests verify migration workflow behavior, OpenAPI metadata coverage, contract-comment visibility, and docs route parity/restriction.
