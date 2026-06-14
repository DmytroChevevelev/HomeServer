# Feature Specification: Device List with Bootstrap and Status Filter

**Feature Branch**: `006-device-list-bootstrap`

**Created**: 2026-05-22

**Status**: Draft

**Input**: User description: "The list of devices should display all registered devices in the database. Use /api/devices endpoint from backend. The list should have filter by status of device. Angular template staff MUST be removed. Use style of control like standard bootstrap UI."

## User Scenarios & Testing *(mandatory)*

### User Story 1 - View All Registered Devices in Bootstrap Layout (Priority: P1)

As an operator, I can open the main page and see a Bootstrap-styled list of all registered devices fetched from the backend, so I can monitor all devices at a glance without any Angular placeholder content.

**Why this priority**: The primary purpose of the dashboard is to surface all registered devices. Without this, the application has no operator value and the Angular default template blocks usability.

**Independent Test**: Open the main page, confirm the Angular default template is gone, and verify device rows are rendered using Bootstrap table/card/badge components with data from `/api/devices`.

**Acceptance Scenarios**:

1. **Given** the application is opened, **When** the main page loads, **Then** the Angular default template placeholder content is not visible and a Bootstrap-styled device list is rendered instead.
2. **Given** registered devices exist in the backend, **When** the main page data loads, **Then** each device row shows name, external ID, device type, and a Bootstrap status badge.
3. **Given** the backend returns an empty device list, **When** the page loads, **Then** an empty-state message is shown using a Bootstrap alert or card.
4. **Given** the backend returns an error, **When** the page loads, **Then** an error message is shown using a Bootstrap alert with appropriate styling.

---

### User Story 2 - Filter Device List by Status (Priority: P2)

As an operator, I can select a device status filter on the main page and immediately see only devices matching that status, so I can quickly isolate online, offline, or warning devices.

**Why this priority**: Status filtering is critical for operational triage; operators need to pinpoint devices with a specific status without scrolling through every entry.

**Independent Test**: With devices of multiple statuses available, select a status filter value and verify the list updates to show only matching devices; reset and verify all devices return.

**Acceptance Scenarios**:

1. **Given** devices of multiple statuses exist, **When** the operator selects a status filter value (e.g., "offline"), **Then** only devices with that status are displayed.
2. **Given** a status filter is active, **When** the operator resets the filter to "All", **Then** all devices are shown again.
3. **Given** the filter is set to a value that matches no devices, **When** the filter is applied, **Then** an empty-result Bootstrap alert or message is shown.

---

### Edge Cases

- Devices exist with status values outside the known set (online/offline/warning/unknown); they must render with a neutral Bootstrap badge and appear under an "Unknown" filter bucket.
- Backend is unavailable at load time; the page must show an error state using a Bootstrap alert without crashing, and filter controls should still render.
- Status filter changes rapidly; the list must update without flickering or showing stale intermediate data.
- Device list loads while filter is already active; only devices matching the active filter must be shown immediately.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: System MUST remove all Angular default template placeholder content from `app.component.html` and replace it with a minimal router outlet shell.
- **FR-002**: System MUST install Bootstrap as a project dependency and make its utility classes available globally to the Angular application.
- **FR-003**: System MUST render the device list using Bootstrap UI components (table, badge, alert, button/button-group) without custom structural styles.
- **FR-004**: System MUST fetch all registered devices from `GET /api/devices` and display them in the Bootstrap-styled list on the main page.
- **FR-005**: Each device row MUST display at minimum: name, external ID, device type, and status as a Bootstrap badge colored by status (success for online, warning for warning, danger for offline, secondary for unknown).
- **FR-006**: System MUST provide a Bootstrap-styled status filter control (dropdown or button group) allowing selection of: All, Online, Offline, Warning, Unknown.
- **FR-007**: The status filter MUST default to "All" and show all devices on initial load.
- **FR-008**: Status filtering MUST operate client-side on the already-fetched device list; no additional API calls per filter change.
- **FR-009**: System MUST display a Bootstrap spinner or loading indicator while devices are being fetched.
- **FR-010**: System MUST show a Bootstrap alert with a descriptive message when the device list is empty or when the backend returns an error.

### Key Entities

- **Device Row**: Single device entry in the list showing name, external ID, type, and Bootstrap status badge.
- **Status Filter State**: Currently selected status value used to restrict visible device rows; one of: all, online, offline, warning, unknown.

## Constitution Alignment *(mandatory)*

- **MVP Slice**: Remove Angular placeholder, install Bootstrap, fetch and display devices from `/api/devices` in a Bootstrap table with status badges. Fully demonstrable with backend running.
- **API Contracts**: Consumes existing `GET /api/devices` endpoint. No backend changes required. Frontend must tolerate any status value not in the known set.
- **Testing Scope**: Minimum tests required — component renders Bootstrap device list with mocked data; status filter correctly limits visible rows; empty-state and error-state render Bootstrap alert.
- **Observability**: API call failures must surface a Bootstrap alert with a user-readable message; errors must not be silently swallowed.
- **Security/Configuration**: No new secrets or config. API base URL uses existing `environment.apiBaseUrl`. Bootstrap is a static dependency with no additional security surface.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: 100% of page loads with backend data result in a Bootstrap-styled device table with no Angular placeholder content visible.
- **SC-002**: 100% of status filter interactions show only matching devices or an empty-state Bootstrap alert; no stale data is displayed.
- **SC-003**: Empty-state and error-state surfaces show a Bootstrap alert in 100% of tested failure cases.
- **SC-004**: Bootstrap classes resolve correctly in production build and apply visual styles to device list controls without any missing-class warnings.

## Assumptions

- Bootstrap will be added via npm and imported globally through `angular.json` styles array or `styles.css`.
- No other component library (e.g., Angular Material) is in active use for these device list pages; Bootstrap is the sole component library for this feature.
- Device status values from the API include at minimum: `online`, `offline`, `warning`; any other value is normalized to `unknown`.
- Client-side filtering is sufficient for the current data set; server-side filtering is out of scope for this increment.
- Angular built-in directives (`*ngIf`, `*ngFor`, `[ngClass]`) are acceptable for conditional rendering and dynamic class binding.
- The `DeviceListComponent` shell from feature 005 will be completed and replaced by this feature's implementation.
