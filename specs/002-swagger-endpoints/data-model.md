# Data Model: Backend Swagger Endpoint Discovery

This feature does not introduce domain persistence changes. It introduces documentation-facing models and configuration-driven behavior.

## Entities

### EndpointDescription
- Purpose: Represents one API route shown in generated documentation.
- Fields:
  - path (string, required)
  - method (string, required, enum: GET|POST|PUT|PATCH|DELETE)
  - operationId (string, optional)
  - summary (string, optional)
  - requestSchemaRef (string, optional)
  - responseSchemas (map<statusCode, schemaRef>, optional)
- Validation:
  - path must start with '/'
  - method must be one of the supported HTTP verbs

### ApiContractDocument
- Purpose: Machine-readable OpenAPI contract used by tools/tests.
- Fields:
  - version (string, required)
  - infoTitle (string, required)
  - servers (list<string>, optional)
  - paths (collection<EndpointDescription>, required)
  - components (schemas map, optional)
- Validation:
  - version and infoTitle must be non-empty
  - paths collection must not contain duplicate method+path pairs

### DocumentationAccessPolicy
- Purpose: Controls when docs endpoints are exposed.
- Fields:
  - environmentName (string, required)
  - docsEnabled (boolean, required)
  - uiPath (string, required)
  - jsonPath (string, required)
- Validation:
  - uiPath and jsonPath must be absolute paths and unique

## Relationships
- ApiContractDocument contains many EndpointDescription records.
- DocumentationAccessPolicy governs whether ApiContractDocument is externally accessible.

## State Transitions

### Documentation Exposure State
- Disabled -> Enabled
  - Trigger: Application starts in an allowed environment/configuration.
- Enabled -> Disabled
  - Trigger: Application starts in a restricted environment or invalid docs configuration is detected.

No EF Core migration is expected for this feature.
