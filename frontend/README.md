# Smart Home Portal Frontend

Angular application for the Smart Home Portal. This UI is intended to work with the backend API and provide a browser-based operator experience.

## Prerequisites

- Node.js 18+ and npm
- Backend API running locally

## Install

```powershell
Set-Location frontend
npm install
```

## Run

```powershell
npm run start
```

The app typically runs with the Angular development server at `http://localhost:4200`.

## Build

```powershell
npm run build
```

## Test

```powershell
npm run test
```

## Notes

- The frontend expects the backend CORS configuration to allow the local Angular origin.
- Backend API documentation is available separately at `/swagger` when the API is running in development mode.
- Device list realtime updates use SignalR hub route `/hubs/telemetry` with event name `sensorValueChanged`.
- If realtime connection is unavailable, the list shows a non-blocking warning and continues to render the latest API projection.

## Device Pages Route Overview

- `/devices`: main device list page (dashboard view)
- `/devices/:deviceId`: device details page with telemetry history section and limit control
- `/devices/register`: device registration page

## Telemetry History Behavior

- Device details call `GET /api/devices/{deviceId}/telemetry?limit={n}` through the facade.
- Default limit is 100 and can be adjusted in the details UI (clamped to 1..500).
- History rows are rendered newest-first according to backend contract fields (`metricType`, `metricValue`, `eventTimeUtc`).