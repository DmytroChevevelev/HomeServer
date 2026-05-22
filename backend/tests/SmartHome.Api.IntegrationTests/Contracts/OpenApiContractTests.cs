using System.Text.Json;
using SmartHome.Api.IntegrationTests.Infrastructure;
using Xunit;

namespace SmartHome.Api.IntegrationTests.Contracts;

public class OpenApiContractTests
{
    [Fact]
    public async Task SwaggerJson_Contains_Required_Backend_Paths()
    {
        using var host = new IntegrationTestHost();
        using var client = host.CreateApiClient();

        var response = await client.GetAsync("/swagger/v1/swagger.json");
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync();
        using var document = await JsonDocument.ParseAsync(stream);
        var paths = document.RootElement.GetProperty("paths");

        Assert.True(paths.TryGetProperty("/api/devices", out _));
        Assert.True(paths.TryGetProperty("/api/telemetry", out _));
        Assert.True(paths.TryGetProperty("/api/telemetry/latest", out _));
    }

    [Fact]
    public async Task SwaggerJson_Contains_RequestBody_For_RegisterDevice()
    {
        using var host = new IntegrationTestHost();
        using var client = host.CreateApiClient();

        var response = await client.GetAsync("/swagger/v1/swagger.json");
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync();
        using var document = await JsonDocument.ParseAsync(stream);

        var devicesPost = document.RootElement
            .GetProperty("paths")
            .GetProperty("/api/devices")
            .GetProperty("post");

        Assert.True(devicesPost.TryGetProperty("requestBody", out _));
    }

    [Fact]
    public async Task SwaggerJson_Contains_Documented_Responses_For_IngestTelemetry()
    {
        using var host = new IntegrationTestHost();
        using var client = host.CreateApiClient();

        var response = await client.GetAsync("/swagger/v1/swagger.json");
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync();
        using var document = await JsonDocument.ParseAsync(stream);

        var responses = document.RootElement
            .GetProperty("paths")
            .GetProperty("/api/telemetry")
            .GetProperty("post")
            .GetProperty("responses");

        Assert.True(responses.TryGetProperty("202", out _));
        Assert.True(responses.TryGetProperty("400", out _));
        Assert.True(responses.TryGetProperty("404", out _));
        Assert.True(responses.TryGetProperty("429", out _));
    }
}

internal static class OpenApiAssertions
{
    public static async Task<JsonDocument> GetSwaggerDocumentAsync(HttpClient client)
    {
        var response = await client.GetAsync("/swagger/v1/swagger.json");
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync();
        return await JsonDocument.ParseAsync(stream);
    }

    public static JsonElement GetOperation(JsonDocument document, string path, string method)
    {
        return document.RootElement
            .GetProperty("paths")
            .GetProperty(path)
            .GetProperty(method.ToLowerInvariant());
    }

    public static void AssertHasSummary(JsonElement operation)
    {
        Assert.True(operation.TryGetProperty("summary", out var summary));
        Assert.False(string.IsNullOrWhiteSpace(summary.GetString()));
    }

    public static void AssertResponseDescription(JsonElement operation, params string[] statusCodes)
    {
        var responses = operation.GetProperty("responses");
        foreach (var statusCode in statusCodes)
        {
            Assert.True(responses.TryGetProperty(statusCode, out var response));
            Assert.True(response.TryGetProperty("description", out var description));
            Assert.False(string.IsNullOrWhiteSpace(description.GetString()));
        }
    }

    public static void AssertHasRequestBodyDescription(JsonElement operation)
    {
        Assert.True(operation.TryGetProperty("requestBody", out var requestBody));
        Assert.True(requestBody.TryGetProperty("description", out var description));
        Assert.False(string.IsNullOrWhiteSpace(description.GetString()));
    }
}
