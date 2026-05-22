# Contract: CORS Origin Policy for `/api/devices`

## Scope
Defines expected cross-origin behavior for browser requests against `GET /api/devices`.

## Allowed origins (development default)
- `http://localhost:4200`
- `http://localhost:4201`
- `http://127.0.0.1:4200`

## Request
- Method: `GET`
- Path: `/api/devices`
- Header: `Origin: <origin>`

## Response behavior contract

### When origin is allowed
- API response status remains endpoint-defined (`200`, etc.)
- Response MUST include `Access-Control-Allow-Origin` matching request origin

### When origin is not allowed
- API endpoint execution may still return a status code server-side
- Response MUST NOT include `Access-Control-Allow-Origin`
- Browser clients MUST treat the response as blocked by CORS

## Non-goals
- No changes to `/api/devices` JSON schema
- No wildcard-origin support
- No change to non-browser clients
