<!--
Sync Impact Report
- Version change: template -> 1.0.0
- Modified principles: Initial adoption (all principles newly defined)
- Added sections: Engineering Standards; Workflow & Quality Gates
- Removed sections: None
- Templates requiring updates:
  - ✅ .specify/templates/plan-template.md
  - ✅ .specify/templates/spec-template.md
  - ✅ .specify/templates/tasks-template.md
  - ✅ .github/prompts/speckit.constitution.prompt.md (reviewed, no changes required)
  - ✅ .github/prompts/speckit.plan.prompt.md (reviewed, no changes required)
  - ✅ .github/prompts/speckit.tasks.prompt.md (reviewed, no changes required)
- Follow-up TODOs: None
-->

# Smart Home Portal Constitution

## Core Principles

### I. MVP Vertical Slice First
Every feature MUST be delivered as an end-to-end, independently demonstrable slice from API
through persistence to UI. Work that cannot be validated through a user-visible outcome MUST
not be prioritized ahead of MVP-critical flow. Rationale: this project optimizes for rapid,
verifiable progress and avoids infrastructure-first drift.

### II. Contract-Driven API and Validation
All externally consumed API behavior MUST be defined through explicit request/response contracts,
with deterministic validation and structured error responses. Breaking contract changes MUST be
versioned and documented before implementation. Rationale: Angular UI, simulator, and backend
depend on stable interfaces for safe parallel evolution.

### III. Testable Changes and Data Integrity
Changes affecting domain logic, ingestion, or persistence MUST include automated verification at
the right level (unit, integration, or contract). Schema changes MUST use EF Core migrations and
be reproducible in local environments. Rationale: telemetry correctness and data continuity are
non-negotiable for monitoring credibility.

### IV. Observability by Default
Backend and background processing MUST emit actionable logs and expose health-focused signals
needed to diagnose failures in ingestion, persistence, and API delivery paths. New workflows MUST
define minimum diagnostics before merge. Rationale: operational visibility is required to detect
stale devices, ingestion issues, and regressions quickly.

### V. Secure and Configurable Environments
Configuration MUST be environment-specific, with no secrets committed to source control and with
explicit CORS/origin and connection-string handling. Security-sensitive features MUST fail closed
when misconfigured. Rationale: local-first delivery must not create unsafe defaults for later
production deployment.

## Engineering Standards

- Primary architecture MUST remain .NET 9 ASP.NET Core Web API, Angular frontend, SQL Server
  Express (local default), and .NET simulator unless an amendment is approved.
- Repository changes SHOULD preserve clear separation of concerns across API, UI, simulator,
  and data access layers.
- New dependencies MUST include a short justification in plan or task artifacts.
- Feature artifacts MUST keep traceability from goals -> spec -> plan -> tasks.

## Workflow & Quality Gates

1. Specification MUST define prioritized user stories, measurable outcomes, and explicit
	constraints before planning.
2. Implementation plans MUST pass Constitution Check gates covering MVP slice, contracts,
	testing, observability, and security/configuration.
3. Tasks MUST be grouped by user story and include required validation and migration work
	where applicable.
4. Pull request review MUST confirm constitutional compliance and document any justified
	complexity exceptions.
5. Release readiness for MVP increments MUST be verified through local end-to-end runs
	(API + Angular + SQL Express + simulator).

## Governance

This constitution supersedes conflicting process notes in repository documentation.
Amendments require: (1) a written proposal, (2) impact assessment on templates/workflows,
and (3) approval by project maintainers before merge.

Versioning policy:
- MAJOR for incompatible governance changes or principle removal/redefinition.
- MINOR for new principles/sections or materially expanded guidance.
- PATCH for clarifications and non-semantic wording improvements.

Compliance review expectations:
- Every implementation plan and pull request MUST include a constitution compliance check.
- Non-compliance MUST be resolved before merge or documented as an explicit, time-bound
  exception with owner and remediation plan.

**Version**: 1.0.0 | **Ratified**: 2026-05-22 | **Last Amended**: 2026-05-22
