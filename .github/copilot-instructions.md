<!-- SPECKIT START -->
For additional context about technologies, project structure, and workflow details,
read `specs/008-device-management-updates/plan.md`.
<!-- SPECKIT END -->

- For this feature, add documentation comments to any endpoint or middleware you create or modify. Use `specs/002-swagger-endpoints/contracts/openapi-docs.yaml` as the source of truth for the required documentation routes and their expected behavior. Ensure that each endpoint you implement or update for this feature includes an OpenAPI summary, parameter descriptions when applicable, and response descriptions that match the contract.

- Keep README.md and other non-code documentation files up to date with any relevant information about the database synchronization process or documentation metadata requirements that would be helpful for developers working on this feature or consuming the API.

## Bug Investigation Lesson: Devices Endpoint Integration

- Root cause observed in this repository: the frontend device mapping expected `latestValue`, while `GET /api/devices` returns `latestMetricValue` (and `latestEventTimeUtc`) from the backend contract. This caused valid API responses to appear broken in UI behavior.
- Additional risk observed: request methods can be accidentally changed during fixes (for example, registration must stay `POST`, while list retrieval must stay `GET`).

### Required validation before proposing or applying fixes

- Always compare frontend DTO fields with backend response contracts before changing UI logic. Treat backend contracts as source of truth.
- Always verify HTTP methods against endpoint intent and contract:
	- `GET` for reads (for example, list/query endpoints)
	- `POST` for create/register operations
- Do not conclude a transport/CORS issue until contract and method alignment are verified.
- When adjusting mapping for compatibility, prefer additive support (current contract + legacy aliases) and add/update tests to lock behavior.

## Bug Investigation Lesson: Frontend Fetch Illegal Invocation

- Root cause observed in this repository: passing bare `fetch` as a default function reference (for example, constructor defaults) can lose the expected invocation context in patched browser environments, causing runtime errors like `TypeError: Failed to execute 'fetch' on 'Window': Illegal invocation`.
- Symptom pattern: debugger often stops in fallback return blocks (for example, generic catch branches), while the real failure occurred earlier during `await fetch(...)` or JSON parsing.

### Required validation before proposing or applying fixes

- If you see `Illegal invocation` in browser stack traces, inspect how `fetch` is injected/passed before changing endpoint logic.
- Prefer context-safe wrappers for defaults, for example:
	- `const defaultFetch = (input, init) => globalThis.fetch(input, init)`
- Add targeted logs in catch branches with operation name and inputs (such as `apiBaseUrl`) so fallback UI states preserve root-cause evidence.
- Add or update focused tests for request flows affected by fetch injection changes.