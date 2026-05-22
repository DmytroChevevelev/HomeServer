# Tech stack for Smart Home Portal

Our Smart Home Portal uses a .NET 9 Core Web API backend and an Angular frontend. This combination supports fast MVP delivery while keeping a clear path to production-ready scaling.

## Why .NET 9 Core Web API + Angular?

Building Smart Home Portal with an ASP.NET Core Web API backend and an Angular frontend offers several advantages:

1. **Clear separation of concerns**: The API handles business logic, device data ingestion, and persistence, while Angular focuses on dashboard UX and user workflows.
2. **Mature ecosystem**: .NET 9 and Angular provide strong tooling, long-term maintainability, and broad community support.
3. **Cross-platform development**: Both backend and frontend development run well on Windows, macOS, and Linux.
4. **Incremental architecture**: Start with core device telemetry and monitoring, then add automation, analytics, and alerting.
5. **Production-ready foundation**: The same architecture supports security hardening, background processing, and observability without a full rewrite.

## Core components

The solution includes four primary components:

1. **Backend API**: ASP.NET Core Web API on .NET 9
2. **Frontend UI**: Angular single-page application
3. **Database**: Microsoft SQL Server Express for persistent storage
4. **Sensor simulator**: .NET 9 console application that emits sample sensor telemetry to the API

## Responsibilities

For the MVP:

**Backend API** is responsible for:

- Exposing endpoints to register sensors/devices
- Receiving sensor readings from the simulator
- Returning sensor and telemetry data for the UI
- Persisting data in MS SQL Express

**Angular frontend** is responsible for:

- Device and sensor management screens
- Real-time or near real-time telemetry display
- Basic status indicators and error messages

**Sensor simulator (.NET 9 console app)** is responsible for:

- Generating realistic sample telemetry (temperature, humidity, motion, etc.)
- Sending telemetry payloads to API endpoints at configurable intervals
- Supporting repeatable test runs for local development

## MVP-first implementation approach

To deliver quickly, implement in phases:

**MVP (core telemetry flow):**

- Create API endpoints for sensor registration and telemetry ingestion
- Build Angular views for adding sensors and viewing latest readings
- Store entities and readings in MS SQL Express using EF Core
- Use the sensor simulator to validate end-to-end ingestion

**Extended MVP:**

- Add threshold-based alerts (for example, high temperature)
- Add historical charts and filtering in Angular
- Add background jobs for summaries and stale-device detection
- Add stronger validation and structured error responses

This phased approach keeps implementation fast while preserving a clean architecture for future expansion.

## Local development

### Angular project initialization

When creating a new Angular project from a template, remove demo components and routes that are unrelated to Smart Home Portal features.

Recommended cleanup steps:

1. Remove sample pages and unused starter components.
2. Update routing so only Smart Home Portal routes remain.
3. Update the navigation menu to match MVP features.
4. Build and run after cleanup to catch route errors early.

### Port configuration

The backend API and Angular UI run on separate localhost ports. Port consistency is critical and must be coordinated between these locations:

1. **Backend port** in API launch settings:

   - Example: `http://localhost:5151`
   - This is where the API listens

2. **Frontend port** in Angular serve configuration:

   - Example: `http://localhost:4200`
   - This is where the Angular app runs

3. **API base URL** in Angular environment configuration:

   - Must match the backend port
   - Example: `http://localhost:5151/api`

4. **CORS policy** in backend startup:

   - Must allow the Angular origin
   - Example: allow `http://localhost:4200`

### Database configuration (MS SQL Express)

Use Microsoft SQL Server Express as the default local database for development.

- Configure a local connection string in API settings.
- Use EF Core migrations to create and evolve schema.
- Keep development and production connection strings separate.

Example connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.\\SQLEXPRESS;Database=SmartHomePortalDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

### Sensor simulator configuration

The simulator is a standalone .NET 9 console app used for local and CI test data generation.

- Configure API base URL and send interval in simulator settings.
- Support deterministic test mode for predictable data.
- Log sent payloads and API responses for troubleshooting.

## Future enhancements (post-MVP)

When ready to evolve beyond MVP, this architecture supports:

- Authentication and role-based access control
- Device command and control workflows
- Rules engine for automation scenarios
- Event streaming and message queue integration
- Enhanced observability (structured logs, tracing, metrics)
- Integration and load testing pipelines

## Summary

.NET 9 Core Web API with Angular provides a practical and scalable architecture for Smart Home Portal:

- **MVP**: Device registration and telemetry ingestion/display using MS SQL Express persistence
- **Extended MVP**: Alerts, analytics, and operational hardening
- **Support tooling**: A .NET 9 console sensor simulator enables repeatable end-to-end testing

This stack is simple enough for rapid delivery and strong enough for production-oriented growth.
