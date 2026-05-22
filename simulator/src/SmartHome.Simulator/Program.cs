using Microsoft.Extensions.Configuration;
using SmartHome.Simulator;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var apiBaseUrl = config["ApiBaseUrl"] ?? "http://localhost:5151/api";
var interval = int.TryParse(config["SendIntervalSeconds"], out var seconds) ? seconds : 2;
var deviceExternalId = config["DeviceExternalId"] ?? "device-001";

using var client = new HttpClient();
var publisher = new TelemetryPublisher(client);
using var cts = new CancellationTokenSource();

Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true;
    cts.Cancel();
};

await publisher.SendAsync(apiBaseUrl, deviceExternalId, interval, cts.Token);
