# Data Model: Fix Devices CORS

## Overview
This feature introduces no persistence schema changes. The relevant model is configuration-driven origin policy.

## Entities

### Development Origin Allowlist Entry
- Representation: string URL origin value under `Cors:AllowedOrigins`
- Examples:
  - `http://localhost:4200`
  - `http://localhost:4201`
  - `http://127.0.0.1:4200`
- Validation rules:
  - Must be an absolute origin (scheme + host + port)
  - No path/query/fragment
  - No wildcard entries

### Origin Access Decision
- Inputs: request `Origin` header, configured allowlist, environment
- Output: CORS allowed or not allowed
- State transitions:
  - Allowed origin -> response includes `Access-Control-Allow-Origin`
  - Disallowed origin -> response omits `Access-Control-Allow-Origin`

### Device List Fetch Attempt
- Endpoint: `GET /api/devices`
- Depends on both endpoint success and origin access decision
- Observable outcomes:
  - Endpoint OK + origin allowed -> browser receives data
  - Endpoint OK + origin disallowed -> browser blocks access
