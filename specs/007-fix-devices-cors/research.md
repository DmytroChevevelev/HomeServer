# Research: Fix Devices CORS

## Decision 1: Expand development CORS allowlist to include common local frontend origins
- Decision: Add `http://localhost:4201` and `http://127.0.0.1:4200` to the existing development allowlist with `http://localhost:4200`.
- Rationale: Browser CORS blocks frontend fetch when dev server runs on fallback port/host variant; backend endpoint itself remains healthy.
- Alternatives considered: wildcard origins (`*`) rejected due to security posture and constitution requirement for explicit configuration.

## Decision 2: Keep explicit allowlist behavior in all environments
- Decision: Continue using `policy.WithOrigins(allowedOrigins)` with environment-specific configuration; do not relax non-development behavior.
- Rationale: Preserves secure-by-default controls and avoids accidental production exposure.
- Alternatives considered: dynamic reflection of `Origin` header rejected due to risk and non-deterministic policy behavior.

## Decision 3: Add automated origin-policy verification in integration tests
- Decision: Add integration tests that assert allowed origins receive CORS response headers and disallowed origins do not for `/api/devices`.
- Rationale: Prevents future regressions where endpoint health appears valid in Swagger but browser clients fail.
- Alternatives considered: manual postman/browser checks only rejected because they are non-repeatable and easy to miss.

## Decision 4: Document CORS troubleshooting in developer docs
- Decision: Update development docs to include supported frontend origins and quick triage for CORS-blocked `/api/devices` calls.
- Rationale: Reduces onboarding/support friction and aligns with observability and operability principles.
- Alternatives considered: relying on inline code comments only rejected because runtime troubleshooting guidance belongs in docs.
