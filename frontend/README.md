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
