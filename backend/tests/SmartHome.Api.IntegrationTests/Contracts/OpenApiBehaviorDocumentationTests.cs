using SmartHome.Api.IntegrationTests.Infrastructure;
using Xunit;

namespace SmartHome.Api.IntegrationTests.Contracts;

public class OpenApiBehaviorDocumentationTests
{
    [Fact]
    public async Task Updated_Routes_Are_Interpretable_From_OpenApi_Documentation_Alone()
    {
        using var host = new IntegrationTestHost();
        using var client = host.CreateApiClient();
        using var document = await OpenApiAssertions.GetSwaggerDocumentAsync(client);

        var registerOperation = OpenApiAssertions.GetOperation(document, "/api/devices", "post");
        Assert.True(registerOperation.TryGetProperty("description", out var registerDescription));
        Assert.False(string.IsNullOrWhiteSpace(registerDescription.GetString()));
        OpenApiAssertions.AssertHasRequestBodyDescription(registerOperation);
        OpenApiAssertions.AssertResponseDescription(registerOperation, "201", "400", "409");

        var ingestOperation = OpenApiAssertions.GetOperation(document, "/api/telemetry", "post");
        Assert.True(ingestOperation.TryGetProperty("description", out var ingestDescription));
        Assert.False(string.IsNullOrWhiteSpace(ingestDescription.GetString()));
        OpenApiAssertions.AssertHasRequestBodyDescription(ingestOperation);
        OpenApiAssertions.AssertResponseDescription(ingestOperation, "202", "400", "404", "429");
    }
}
