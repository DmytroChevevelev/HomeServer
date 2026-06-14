# Implementation Plan: Fix Devices CORS

**Branch**: `007-fix-devices-cors` | **Date**: 2026-05-22 | **Spec**: [spec.md](spec.md)

**Input**: Feature specification from `specs/007-fix-devices-cors/spec.md`

## Summary

Fix frontend device-list fetch failures caused by development CORS origin mismatch while preserving strict explicit allowlist behavior. The implementation updates development origin configuration, adds origin-policy integration tests for `/api/devices`, and documents troubleshooting so developers can quickly resolve CORS blocks.

## Technical Context

**Language/Version**: C# (.NET 9), JSON configuration, Markdown docs

**Primary Dependencies**: ASP.NET Core CORS middleware, xUnit integration test host (`SmartHome.Api.IntegrationTests`)

**Storage**: N/A (no data model persistence changes)

**Testing**: xUnit integration tests for origin policy behavior on `/api/devices`

**Target Platform**: Local development backend (`SmartHome.Api`) consumed by Angular frontend

**Project Type**: Web application (backend API + frontend SPA)

**Performance Goals**: No measurable runtime impact; maintain current request handling characteristics

**Constraints**: No wildcard origin policy; keep environment-specific explicit allowlist controls

**Scale/Scope**: Single backend policy area plus integration tests and developer documentation

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- MVP Vertical Slice: PASS. End-to-end user-visible flow is restored (browser frontend can fetch `/api/devices` from supported local origins).
- API Contracts: PASS. No request/response payload changes; origin policy behavior is clarified and validated.
- Test and Data Integrity: PASS. Integration tests are included; no schema/migration impact.
- Observability: PASS. Troubleshooting guidance and test evidence distinguish origin blocks from endpoint/data issues.
- Security and Configuration: PASS. Explicit allowlist retained; no wildcard expansion.

No constitution violations detected.

## Project Structure

### Documentation (this feature)

```text
specs/007-fix-devices-cors/
├── plan.md
├── research.md
├── data-model.md
├── quickstart.md
├── contracts/
│   └── cors-origin-policy.md
└── tasks.md
```

### Source Code (repository root)

```text
backend/
├── src/
│   └── SmartHome.Api/
│       ├── Program.cs
│       └── appsettings.Development.json
└── tests/
    └── SmartHome.Api.IntegrationTests/
        └── Contracts/

docs/
└── development-commands.md
```

**Structure Decision**: Use existing web-application layout; changes limited to backend configuration/policy wiring, backend integration tests, and docs.

## Phase 0: Research Output

Research decisions captured in [research.md](research.md):
- Expand development allowlist for expected local origins.
- Preserve strict explicit allowlist behavior in all environments.
- Add integration tests for allowed vs disallowed origins.
- Document troubleshooting path for CORS vs endpoint failures.

## Phase 1: Design Output

- Data model: [data-model.md](data-model.md)
- Contract: [contracts/cors-origin-policy.md](contracts/cors-origin-policy.md)
- Validation runbook: [quickstart.md](quickstart.md)

Post-design constitution re-check: PASS across all gates.

## Phase 2: Implementation Preview

1. Update development origin allowlist values in `appsettings.Development.json`.
2. Ensure CORS policy continues to use explicit `WithOrigins(...)` behavior.
3. Add/extend integration tests validating CORS header behavior for supported and unsupported origins on `/api/devices`.
4. Update developer docs for supported origins and troubleshooting.
5. Run integration test suite and verify no API contract regressions.

## Complexity Tracking

No justified complexity exceptions required.
