using SmartHome.Api.IntegrationTests.Infrastructure;
using Xunit;

namespace SmartHome.Api.IntegrationTests.Contracts;

public class OpenApiDevicesMetadataTests
{
    [Fact]
    public async Task Devices_Operations_Have_Required_OpenApi_Metadata()
    {
        using var host = new IntegrationTestHost();
        using var client = host.CreateApiClient();
        using var document = await OpenApiAssertions.GetSwaggerDocumentAsync(client);

        var registerOperation = OpenApiAssertions.GetOperation(document, "/api/devices", "post");
        OpenApiAssertions.AssertHasSummary(registerOperation);
        OpenApiAssertions.AssertHasRequestBodyDescription(registerOperation);
        OpenApiAssertions.AssertResponseDescription(registerOperation, "201", "400", "409");

        var listOperation = OpenApiAssertions.GetOperation(document, "/api/devices", "get");
        OpenApiAssertions.AssertHasSummary(listOperation);
        OpenApiAssertions.AssertResponseDescription(listOperation, "200");

        var unregisterOperation = OpenApiAssertions.GetOperation(document, "/api/devices/{deviceId}", "delete");
        OpenApiAssertions.AssertHasSummary(unregisterOperation);
        OpenApiAssertions.AssertHasParameterDescription(unregisterOperation, "deviceId");
        OpenApiAssertions.AssertResponseDescription(unregisterOperation, "200", "404");
    }
}
