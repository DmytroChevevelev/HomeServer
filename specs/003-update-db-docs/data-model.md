# Data Model: Database Sync and API Docs Metadata

This feature introduces no new domain tables by itself but formalizes schema synchronization state and documentation metadata expectations.

## Entities

### SchemaMigrationState
- Purpose: Represents migration progression for an environment.
- Fields:
  - migrationId (string, required)
  - appliedAtUtc (datetime, required)
  - environmentName (string, required)
  - status (enum, required: Applied|Failed)
- Validation:
  - migrationId must exist in migration history for successful states.
  - status `Failed` must include an actionable error record in command output/logs.

### EndpointDocumentationMetadata
- Purpose: Represents required OpenAPI metadata for one API operation.
- Fields:
  - path (string, required)
  - method (string, required: GET|POST|PUT|PATCH|DELETE)
  - summary (string, required)
  - parameterDescriptions (collection, optional when no parameters)
  - responseDescriptions (map<statusCode, description>, required)
- Validation:
  - summary must be non-empty.
  - each declared response code must have a non-empty description.
  - parameter descriptions are mandatory when operation parameters exist.

### DocumentationContractExpectation
- Purpose: Captures the source-of-truth behavior expected from docs endpoints and modified operations.
- Fields:
  - contractSource (string, required)
  - docsPaths (collection<string>, required)
  - operationCoverage (collection<EndpointDocumentationMetadata>, required)
- Validation:
  - contractSource must point to a tracked contract artifact.
  - docsPaths must include `/swagger`, `/swagger/index.html`, and `/swagger/v1/swagger.json`.

## Relationships
- DocumentationContractExpectation includes many EndpointDocumentationMetadata entries.
- EndpointDocumentationMetadata for modified routes is validated against generated OpenAPI output.
- SchemaMigrationState reflects the DB schema required by the documented operations.

## State Transitions

### Migration lifecycle
- Pending -> Applied
  - Trigger: `dotnet ef database update` succeeds.
- Pending -> Failed
  - Trigger: migration command exits with error.
- Failed -> Applied
  - Trigger: issue remediated and migration command re-run successfully.

### Documentation compliance lifecycle
- Incomplete -> Complete
  - Trigger: changed endpoint includes summary, parameter descriptions (when applicable), and response descriptions.
- Complete -> Incomplete
  - Trigger: endpoint signature or responses change without corresponding metadata update.
