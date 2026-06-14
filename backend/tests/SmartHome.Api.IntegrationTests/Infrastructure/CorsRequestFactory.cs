namespace SmartHome.Api.IntegrationTests.Infrastructure;

internal static class CorsRequestFactory
{
    public static HttpRequestMessage CreateGetWithOrigin(string path, string origin)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, path);
        request.Headers.Add("Origin", origin);
        return request;
    }
}
