using System.Net.Http.Json;
using System.Text.Json;

namespace SmartHome.Simulator;

public sealed class TelemetryPublisher(HttpClient client)
{
    public async Task SendAsync(string apiBaseUrl, string deviceExternalId, int intervalSeconds, CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            var payload = new
            {
                deviceExternalId,
                metricType = "temperature",
                metricValue = 21.5m,
                eventTimeUtc = DateTime.UtcNow
            };

            var response = await client.PostAsJsonAsync($"{apiBaseUrl}/telemetry", payload, ct);
            var body = await response.Content.ReadAsStringAsync(ct);
            Console.WriteLine(JsonSerializer.Serialize(new { Status = (int)response.StatusCode, Body = body }));
            await Task.Delay(TimeSpan.FromSeconds(intervalSeconds), ct);
        }
    }
}
