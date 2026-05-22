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

## Device Pages Route Overview

- `/devices`: main device list page (dashboard view)
- `/devices/:deviceId`: device details page with history section and date-time filter
- `/devices/register`: device registration page

## TODO:
- The list of devices should display all registered devices in the database. Use /api/devices endpoint from backend.
- The list should have filter by status of device
- Angular template staff MUST be removed
- Use style of control like standard bootstrap UI