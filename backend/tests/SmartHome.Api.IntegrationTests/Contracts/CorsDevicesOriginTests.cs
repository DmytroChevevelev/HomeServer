using SmartHome.Api.IntegrationTests.Infrastructure;
using Xunit;

namespace SmartHome.Api.IntegrationTests.Contracts;

public class CorsDevicesOriginTests
{
    // Guardrail: Program.cs must keep explicit WithOrigins(...) allowlist behavior.
    [Theory]
    [InlineData("http://localhost:4200")]
    [InlineData("http://localhost:4201")]
    [InlineData("http://127.0.0.1:4200")]
    public async Task DevicesEndpoint_Allows_Configured_Development_Origins(string origin)
    {
        using var host = new IntegrationTestHost("Development");
        using var client = host.CreateApiClient();
        using var request = CorsRequestFactory.CreateGetWithOrigin("/api/devices", origin);

        var response = await client.SendAsync(request);

        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        CorsAssertions.AssertAllowsOrigin(response, origin);
    }

    [Fact]
    public async Task DevicesEndpoint_Does_Not_Allow_Disallowed_Origin()
    {
        using var host = new IntegrationTestHost("Development");
        using var client = host.CreateApiClient();
        using var request = CorsRequestFactory.CreateGetWithOrigin("/api/devices", "http://localhost:9999");

        var response = await client.SendAsync(request);

        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        CorsAssertions.AssertDoesNotAllowOrigin(response);
    }

    [Fact]
    public async Task DevicesEndpoint_Does_Not_Allow_Unlisted_Localhost_Regression_Check()
    {
        using var host = new IntegrationTestHost("Development");
        using var client = host.CreateApiClient();
        using var request = CorsRequestFactory.CreateGetWithOrigin("/api/devices", "http://localhost:4202");

        var response = await client.SendAsync(request);

        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        CorsAssertions.AssertDoesNotAllowOrigin(response);
    }
}
