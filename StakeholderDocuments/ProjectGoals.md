# Project goals

Build Smart Home Portal as a practical MVP that proves end-to-end telemetry flow from simulated devices to a web dashboard.

## Purpose

The project demonstrates a clean, scalable baseline for IoT-style monitoring:

- Register sensors/devices
- Ingest telemetry readings
- Persist readings in a relational database
- Display current device status and latest telemetry in the UI

The immediate goal is a working local system with clear extension paths for alerts, automation, and operations hardening.

## Target scope (MVP only)

This is a local, single-tenant MVP designed for fast development and testing on Windows, macOS, or Linux.

The MVP includes:

- ASP.NET Core Web API on .NET 9 for device registration and telemetry ingestion
- Angular frontend for device management and telemetry views
- Microsoft SQL Server Express persistence via EF Core
- .NET 9 console-based sensor simulator for repeatable telemetry generation

MVP user outcomes:

1. User can register a sensor/device through the application workflow.
2. Simulator can send telemetry to API endpoints at configurable intervals.
3. Telemetry is stored in SQL Server Express and returned by API queries.
4. UI shows devices and latest readings in near real-time.

## Delivery approach

Delivery prioritizes an end-to-end vertical slice before feature depth.

Phase 1 (core telemetry flow):

- Implement registration and telemetry ingestion endpoints.
- Add Angular screens for sensor creation and latest readings.
- Persist entities and readings using EF Core migrations.
- Validate ingestion path using simulator-generated data.

Phase 2 (extended MVP):

- Add threshold-based alerts.
- Add historical telemetry charts and filtering.
- Add background summaries and stale-device detection.
- Strengthen validation and structured API errors.

## What "MVP working" means

The MVP is complete when:

1. API, Angular app, SQL Express, and simulator run together locally.
2. A sensor/device can be registered and listed.
3. Simulator telemetry is accepted by the API and persisted.
4. The UI reflects newly ingested data without manual data seeding.

## Local development checklist

Before validating MVP behavior, verify:

- [ ] Backend runs and listens on the configured localhost port (for example, 5151)
- [ ] Frontend runs on its configured localhost port (for example, 4200)
- [ ] Angular environment API base URL matches backend port and `/api` path
- [ ] Backend CORS policy allows the Angular origin
- [ ] SQL Express connection string is configured correctly
- [ ] EF Core migrations are applied successfully
- [ ] Simulator targets the correct API base URL and send interval

## Future enhancements (post-MVP)

Once the extended MVP is stable, evolve the platform with:

- Authentication and role-based authorization
- Device command and control workflows
- Rules engine for automation scenarios
- Event streaming or message queue integration
- Structured logging, tracing, and metrics
- Integration, performance, and load testing pipelines

## Technology alignment note

The selected stack (.NET 9 Web API, Angular, SQL Server Express, and .NET simulator) is intentionally MVP-friendly while remaining production-oriented. It supports incremental growth without a rewrite and maintains clear separation of concerns across API, UI, persistence, and data generation.

## Document alignment

- [TechStack.md](TechStack.md) defines architecture, responsibilities, and phased implementation details
