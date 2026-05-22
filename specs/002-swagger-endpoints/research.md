# Research: Backend Swagger Endpoint Discovery

## Decision 1: Use Swashbuckle as the OpenAPI provider
- Decision: Add Swagger/OpenAPI generation using the Swashbuckle ASP.NET Core package for the existing minimal API service.
- Rationale: It is the standard .NET ecosystem path for minimal APIs, keeps onboarding simple, and integrates directly with endpoint metadata.
- Alternatives considered: Manual OpenAPI document maintenance only; NSwag-first setup for generation. These were rejected for MVP because they add process overhead or duplicate effort with the existing backend style.

## Decision 2: Expose interactive docs and machine-readable contract in development-first mode
- Decision: Serve an interactive docs UI and JSON OpenAPI document in development-oriented environments.
- Rationale: The feature goal is endpoint discoverability and testability during active development.
- Alternatives considered: Production-wide exposure by default; docs disabled everywhere. Default production exposure was rejected due to security risk, and always-disabled was rejected because it fails the feature goal.

## Decision 3: Restrict documentation exposure outside allowed environments
- Decision: Documentation endpoints are disabled by policy outside allowed environments, returning a clear unavailable/not-found behavior.
- Rationale: Aligns with constitution security and safe-default requirements.
- Alternatives considered: Relying on network isolation only; obscuring URL paths. Both are weaker controls and do not provide an explicit fail-safe behavior.

## Decision 4: Keep API contract visibility and endpoint docs synchronized through automated checks
- Decision: Add integration checks for documentation endpoint availability and machine-readable document retrieval, and extend contract tests to detect drift.
- Rationale: The project already uses integration and contract testing; this extends confidence to docs discoverability.
- Alternatives considered: Manual QA-only checks. Rejected because drift can go unnoticed between releases.

## Decision 5: No domain persistence/schema changes are required
- Decision: Treat this as an API-surface and configuration feature with no new EF Core migrations.
- Rationale: Swagger introduces discoverability artifacts, not business data entities.
- Alternatives considered: Persisting docs telemetry in database. Rejected for MVP due to unnecessary complexity.

## Decision 6: Add minimum diagnostics for docs requests and execution failures
- Decision: Capture logs for documentation endpoint requests and failed interactive request attempts.
- Rationale: Supports troubleshooting if docs are unavailable or not reflecting expected behavior.
- Alternatives considered: No dedicated diagnostics. Rejected because observability is a constitution gate and needed for supportability.

## Resolved Clarifications
- No unresolved NEEDS CLARIFICATION items remain for this feature.

## Contract Drift Check Guidance
- Run `dotnet test backend/tests/SmartHome.Api.IntegrationTests/SmartHome.Api.IntegrationTests.csproj`.
- Ensure `OpenApiContractTests` validates required paths and documented telemetry responses.
- When endpoint metadata changes (names, summaries, responses), update test expectations in the same change set.
