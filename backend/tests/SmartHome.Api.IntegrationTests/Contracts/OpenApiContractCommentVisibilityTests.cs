using SmartHome.Api.IntegrationTests.Infrastructure;
using Xunit;

namespace SmartHome.Api.IntegrationTests.Contracts;

public class OpenApiContractCommentVisibilityTests
{
    [Fact]
    public async Task Contract_Property_Documentation_Is_Visible_In_OpenApi_Schemas()
    {
        using var host = new IntegrationTestHost();
        using var client = host.CreateApiClient();
        using var document = await OpenApiAssertions.GetSwaggerDocumentAsync(client);

        var schemas = document.RootElement.GetProperty("components").GetProperty("schemas");

        var registerSchema = schemas.GetProperty("RegisterDeviceRequest");
        var registerProperties = registerSchema.GetProperty("properties");
        Assert.False(string.IsNullOrWhiteSpace(registerProperties.GetProperty("externalId").GetProperty("description").GetString()));
        Assert.False(string.IsNullOrWhiteSpace(registerProperties.GetProperty("name").GetProperty("description").GetString()));
        Assert.False(string.IsNullOrWhiteSpace(registerProperties.GetProperty("sensorType").GetProperty("description").GetString()));

        var ingestSchema = schemas.GetProperty("IngestTelemetryRequest");
        var ingestProperties = ingestSchema.GetProperty("properties");
        Assert.False(string.IsNullOrWhiteSpace(ingestProperties.GetProperty("deviceExternalId").GetProperty("description").GetString()));
        Assert.False(string.IsNullOrWhiteSpace(ingestProperties.GetProperty("metricType").GetProperty("description").GetString()));
        Assert.False(string.IsNullOrWhiteSpace(ingestProperties.GetProperty("metricValue").GetProperty("description").GetString()));
    }
}
