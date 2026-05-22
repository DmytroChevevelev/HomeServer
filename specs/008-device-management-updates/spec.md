# Feature Specification: Device Management Updates

**Feature Branch**: `[008-device-management-updates]`

**Created**: 2026-05-23

**Status**: Draft

**Input**: User description:

- Add telemetry data
- Add error handling and logging. For .NET project use Serilog, for Angular - log to console
- Implement Device details page to show device data and telemetry. Add button to unregister device. Navigation to this page should be on click on device name.
- Add Unregister device endpoint to the backend
- Update Add Device page to support all fields in contract to properly fill database table.
- Each device row on devices list page should be expandable to show all device metadata. The most recent value of telemetry for device should be displayed in visible row and be updated when backend receives new value

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Rich Device List (Priority: P1)

As an operator, I can open the devices page, see every device with its current summary data, expand a row to inspect all available metadata, and view the latest telemetry value directly in the visible row.

**Why this priority**: The list page is the primary entry point for day-to-day device monitoring, so it must provide the highest-value overview first.

**Independent Test**: Can be verified by opening the devices page, confirming that each row shows a live summary, expanding a row, and checking that all metadata is visible without navigating away.

**Acceptance Scenarios**:

1. **Given** devices exist with telemetry history, **When** I open the devices page, **Then** I see a row for each device with the latest telemetry value and status.
2. **Given** a device row is visible, **When** I expand the row, **Then** I can view all metadata fields available for that device.
3. **Given** telemetry is ingested for a device, **When** the page refreshes or the device list is reloaded, **Then** the most recent telemetry value shown for that device reflects the newest reading.

---

### User Story 2 - Device Details and Removal (Priority: P1)

As an operator, I can click a device name to open a details page that shows the device profile and telemetry, and I can unregister the device from that page when it is no longer needed.

**Why this priority**: Device investigation and cleanup are core management tasks and must be available without relying on the list view alone.

**Independent Test**: Can be verified by selecting a device name, confirming the details page opens, and unregistering the device successfully.

**Acceptance Scenarios**:

1. **Given** a device exists in the list, **When** I click its name, **Then** I navigate to a dedicated details page for that device.
2. **Given** a device details page is open, **When** I review the page, **Then** I can see the device profile and telemetry information for that device.
3. **Given** a device details page is open, **When** I choose to unregister the device and confirm the action, **Then** the device is removed from the system and is no longer available in the devices list.

---

### User Story 3 - Complete Device Registration (Priority: P2)

As an operator, I can register a device using all fields required by the device contract so that the backend stores a complete and correct device record.

**Why this priority**: Registration is necessary for new devices, but it becomes most valuable once list and details workflows are in place.

**Independent Test**: Can be verified by submitting a complete device registration form and checking that the stored device data includes every required contract field.

**Acceptance Scenarios**:

1. **Given** I open the Add Device page, **When** I enter all required contract fields and submit the form, **Then** the device is registered successfully.
2. **Given** a required field is missing or invalid, **When** I submit the form, **Then** I see a clear validation or error message and no incomplete device record is created.

---

### User Story 4 - Telemetry Visibility and Delivery (Priority: P2)

As an operator or simulator user, I can add telemetry data for a device and rely on the UI to surface that data in the list and details experiences.

**Why this priority**: Telemetry makes the device pages useful beyond static inventory by showing live device behavior.

**Independent Test**: Can be verified by adding telemetry for a device and confirming that the latest telemetry value appears on the list and in the device detail experience after the next refresh or reload cycle.

**Acceptance Scenarios**:

1. **Given** a device has telemetry readings, **When** the backend stores a newer reading, **Then** the latest reading is the one surfaced in the device summary.
2. **Given** a device has no telemetry history yet, **When** I view the list or details page, **Then** the UI clearly indicates that no telemetry value is available.

---

### Edge Cases

- A device may have no telemetry yet; the UI should show an explicit empty-state value instead of a broken or misleading reading.
- A device may be expanded in the list while the latest telemetry is unavailable; the row must still render the rest of the metadata.
- A device unregister request may fail because the device no longer exists or the backend is unavailable; the user should see a clear error and the page should remain usable.
- Registration can fail due to missing required fields or duplicate external identifiers; the UI should preserve the form and show the failure reason.
- Telemetry payloads may arrive with missing optional values; the UI should tolerate them and continue rendering the device row or details page.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: The system MUST display a devices list that includes each device's summary data and the latest telemetry value available for that device.
- **FR-002**: The system MUST allow each devices-list row to expand and reveal all device metadata available for that device.
- **FR-003**: The system MUST open a device details page when the user clicks a device name in the devices list.
- **FR-004**: The device details page MUST show the selected device's profile data and telemetry information.
- **FR-005**: The system MUST provide an unregister action for a device from the device details experience.
- **FR-006**: The backend MUST expose an unregister capability that removes the selected device and returns a clear success or failure outcome.
- **FR-007**: The Add Device page MUST collect and submit every field required by the device contract so the backend can persist a complete device record.
- **FR-008**: The system MUST accept and surface telemetry data so that the latest telemetry value for a device can be displayed in the list and details experiences.
- **FR-009**: The frontend MUST provide visible error feedback when loading devices, loading details, registering a device, unregistering a device, or loading telemetry fails.
- **FR-010**: The frontend MUST log operational failures to the browser console with enough context to identify the failed operation.
- **FR-011**: The backend MUST emit structured logs for registration, unregister, telemetry ingestion, and device projection failures.
- **FR-012**: The system MUST preserve compatibility with the existing device contract by treating backend field names and HTTP methods as the source of truth.

### Key Entities *(include if feature involves data)*

- **Device**: A registered smart-home device with identity, display name, sensor type, registration state, and metadata used by the list and details views.
- **TelemetryReading**: A timestamped measurement associated with a device, including metric type and metric value.
- **DeviceSummary**: The list-page representation of a device, including status and the latest telemetry value.
- **DeviceDetailsView**: The details-page representation of a device, combining profile data with telemetry information and available actions.
- **DeviceRegistrationPayload**: The set of contract fields submitted when creating a device record.

## Constitution Alignment *(mandatory)*

- **MVP Slice**: A user can open the devices list, inspect row metadata, open a device details page, and unregister a device while seeing the latest telemetry value in the visible row.
- **API Contracts**: The feature depends on `GET /api/devices` for the list projection, `POST /api/devices` for device registration, and a new `DELETE /api/devices/{deviceId}` endpoint for unregistering devices. Existing telemetry reads must continue to provide the latest device telemetry consumed by the list and details views.
- **Testing Scope**: Add unit tests for list/detail mapping, expand/collapse behavior, form validation, error handling, and telemetry value rendering; add integration tests for list retrieval, registration, unregister, and telemetry-driven projection updates; add contract tests to confirm the response shape and HTTP methods remain aligned with the backend contract.
- **Observability**: Frontend request failures should be logged to the console with the operation name and API base URL; backend failures should be logged with structured context for device identity, telemetry ingestion, unregister operations, and projection generation.
- **Security/Configuration**: The feature continues to rely on the existing API base URL and CORS allowlist. Logs must not expose secrets. Misconfigured origins, missing telemetry data, or unavailable backend services should fail safely with a visible user error rather than a broken page.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: Users can identify a device's latest telemetry value from the list page without opening another page.
- **SC-002**: Users can expand a device row to view all available metadata in one interaction flow.
- **SC-003**: Users can open a device details page by clicking a device name and can unregister the device from that page without encountering a blank or broken screen.
- **SC-004**: Users can register a device with all required contract fields and receive a clear success or failure result.
- **SC-005**: When telemetry data changes, the updated latest value is reflected in the device summary after the next refresh or reload cycle.
- **SC-006**: Failed list, detail, registration, unregister, and telemetry operations produce visible user feedback and an operational log entry.

## Assumptions

- Existing device and telemetry persistence remains the source of truth for list and details views.
- Real-time push updates are out of scope unless the existing backend already provides them; refresh or reload-based visibility is acceptable for the initial slice.
- The unregister flow is limited to the current device-management experience and does not require bulk actions.
- The Add Device page reuses the existing frontend application shell and validation patterns rather than introducing a new workflow.
- Existing CORS and API base URL configuration remain in place and only need to support the new unregister and telemetry-related flows.