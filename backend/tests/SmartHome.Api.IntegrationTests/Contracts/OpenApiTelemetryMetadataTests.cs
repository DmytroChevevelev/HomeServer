using SmartHome.Api.IntegrationTests.Infrastructure;
using Xunit;

namespace SmartHome.Api.IntegrationTests.Contracts;

public class OpenApiTelemetryMetadataTests
{
    [Fact]
    public async Task Telemetry_Operations_Have_Required_OpenApi_Metadata()
    {
        using var host = new IntegrationTestHost();
        using var client = host.CreateApiClient();
        using var document = await OpenApiAssertions.GetSwaggerDocumentAsync(client);

        var ingestOperation = OpenApiAssertions.GetOperation(document, "/api/telemetry", "post");
        OpenApiAssertions.AssertHasSummary(ingestOperation);
        OpenApiAssertions.AssertHasRequestBodyDescription(ingestOperation);
        OpenApiAssertions.AssertResponseDescription(ingestOperation, "202", "400", "404", "429");

        var latestOperation = OpenApiAssertions.GetOperation(document, "/api/telemetry/latest", "get");
        OpenApiAssertions.AssertHasSummary(latestOperation);
        OpenApiAssertions.AssertResponseDescription(latestOperation, "200");
    }
}
