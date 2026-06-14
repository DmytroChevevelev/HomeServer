# App features

Smart Home Portal focuses on device onboarding and telemetry visibility as the core MVP user experience.

## MVP scope (core telemetry flow)

The MVP delivers a complete local workflow from device registration to dashboard display.

For MVP, the system MUST provide:

- Device/sensor registration through API and UI workflow
- Telemetry ingestion endpoint that accepts simulator payloads
- Persistent storage of devices and readings in SQL Server Express
- Device list and latest-reading views in the Angular app
- Basic API and UI error states for invalid input and failed requests

For MVP, the system SHOULD provide:

- Configurable simulator send interval
- Near real-time refresh of latest telemetry in the UI
- Simple status indicators (online/recently active/stale)

For MVP, the system MAY defer:

- Advanced analytics and charting
- Complex role and permission models
- Production deployment automation

## MVP behavior

The MVP follows these core behaviors:

- A user can create/register a device and see it appear in the UI list.
- The simulator posts telemetry to the API at configured intervals.
- The API validates payload shape, stores records, and returns appropriate responses.
- The UI displays newly ingested readings without manual database interaction.
- Basic failures are surfaced with clear, actionable messages.

## Extended-MVP features

After core telemetry flow is stable, Extended-MVP adds operational value:

- **Threshold alerts**: Highlight out-of-range telemetry (for example, high temperature).
- **Historical telemetry views**: Add charts and time-range filtering.
- **Background processing**: Compute summaries and detect stale devices.
- **Stronger API contracts**: Structured validation errors and standardized response envelopes.

## Post-MVP features

After Extended-MVP, prioritize production-oriented capabilities.

### Essential improvements

- **Authentication and authorization**: Secure access with user identity and roles.
- **Device command workflows**: Send commands to supported devices and track command status.
- **Rules engine**: Support automation rules based on telemetry conditions.
- **Observability**: Structured logs, metrics, and distributed tracing.

### Additional capabilities

- **Event streaming integration**: Add queue/event bus support for higher scale.
- **Fleet health insights**: Reliability reports and anomaly detection.
- **Notification channels**: Email/SMS/webhook alerts.
- **Performance and load test coverage**: Validate scaling behavior under telemetry bursts.

## Practical implementation notes

**For MVP:**

- Keep API endpoints focused on device CRUD (minimum create/list) and telemetry ingest/query.
- Use EF Core migrations for schema evolution from the start.
- Keep Angular routes limited to device management and latest telemetry screens.
- Use the .NET simulator as the primary test-data source for end-to-end validation.

**For Extended-MVP:**

- Add historical query endpoints with pagination and time filters.
- Introduce background jobs for summaries and stale-device marking.
- Add alert evaluation logic with configurable thresholds.

## Feature acceptance checklist (MVP)

- [ ] Device can be registered and listed through the UI
- [ ] Simulator can post telemetry to API successfully
- [ ] Telemetry rows are persisted in SQL Server Express
- [ ] API returns telemetry for UI consumption
- [ ] UI reflects latest readings and basic device status
- [ ] Validation and request failures surface clear error messages

## How this document aligns

- [ProjectGoals.md](ProjectGoals.md) defines project outcomes and delivery phases
- [TechStack.md](TechStack.md) defines architecture and component responsibilities
