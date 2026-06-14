using Xunit;

namespace SmartHome.Api.IntegrationTests.Infrastructure;

internal static class CorsAssertions
{
    public static void AssertAllowsOrigin(HttpResponseMessage response, string expectedOrigin)
    {
        Assert.True(response.Headers.TryGetValues("Access-Control-Allow-Origin", out var origins));
        Assert.Contains(expectedOrigin, origins);
    }

    public static void AssertDoesNotAllowOrigin(HttpResponseMessage response)
    {
        Assert.False(response.Headers.TryGetValues("Access-Control-Allow-Origin", out _));
    }
}
