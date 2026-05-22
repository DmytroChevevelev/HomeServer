# Research: Database Sync and API Docs Metadata

## Decision 1: Use EF Core migrations as the only schema synchronization mechanism
- Decision: Apply database creation and update through EF Core migration commands instead of manual SQL scripts.
- Rationale: The repository already contains migration history (`0001_Initial`) and the constitution requires reproducible schema changes.
- Alternatives considered: Manual SQL migration scripts and ad hoc table edits were rejected because they are hard to audit and can drift across environments.

## Decision 2: Keep database create and update flow command-identical for local environments
- Decision: Use `dotnet ef database update` for both fresh database provisioning and incremental upgrades.
- Rationale: A single idempotent command reduces operator error and simplifies onboarding.
- Alternatives considered: Separate custom scripts for create vs. update were rejected because they duplicate logic and increase maintenance burden.

## Decision 3: Preserve existing documentation route contract behavior as source of truth
- Decision: Continue to enforce `/swagger`, `/swagger/index.html`, and `/swagger/v1/swagger.json` behavior defined by `specs/002-swagger-endpoints/contracts/openapi-docs.yaml`.
- Rationale: The feature explicitly requires alignment to the existing documentation access contract.
- Alternatives considered: Re-defining docs route behavior in a new contract was rejected to avoid contradictory sources of truth.

## Decision 4: Enforce endpoint documentation completeness for modified routes
- Decision: Any endpoint modified in this feature must include OpenAPI summary, parameter descriptions when parameters exist, and documented responses.
- Rationale: This maps directly to feature requirements and improves consumer confidence.
- Alternatives considered: Optional metadata updates were rejected because they permit contract drift and uneven documentation quality.

## Decision 5: Validate contract/doc consistency using integration contract tests
- Decision: Extend or add integration contract tests that assert required OpenAPI metadata and documented responses for changed operations.
- Rationale: Existing integration test infrastructure already validates Swagger JSON and is the fastest path to automated drift detection.
- Alternatives considered: Manual Swagger UI inspection only was rejected because it is inconsistent and non-blocking for CI.

## Resolved Clarifications
- No unresolved NEEDS CLARIFICATION items remain.
