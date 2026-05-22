using Microsoft.Extensions.Configuration;
using SmartHome.Simulator.Commands;
using SmartHome.Simulator;
using SmartHome.Simulator.Runtime;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var loader = new SimulatorConfigurationLoader();
var profile = loader.Load(config, AppContext.BaseDirectory);

using var client = new HttpClient();
var backendClient = new SimulatorBackendClient(client);
var registerHandler = new RegisterDeviceCommandHandler(backendClient);
var telemetryPublisher = new TelemetryPublisher(backendClient);
var telemetryHandlers = new TelemetryControlCommandHandlers(telemetryPublisher, new TelemetryProfileLoader());
var telemetryState = TelemetrySessionState.Idle;
using var cts = new CancellationTokenSource();

Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true;
    cts.Cancel();
};

Console.WriteLine("Simulator command loop started. Supported commands: register-device, start-send-telemetry, stop-send-telemetry");
while (!cts.Token.IsCancellationRequested)
{
    Console.Write("> ");
    var input = Console.ReadLine();
    if (input is null)
    {
        break;
    }

    var command = input.Trim();
    if (command.Length == 0)
    {
        continue;
    }

    if (string.Equals(command, "register-device", StringComparison.OrdinalIgnoreCase))
    {
        await registerHandler.ExecuteAsync(profile, Console.Out, cts.Token);
        continue;
    }

    if (string.Equals(command, "start-send-telemetry", StringComparison.OrdinalIgnoreCase))
    {
        telemetryState = await telemetryHandlers.HandleStartAsync(profile, telemetryState, Console.Out, cts.Token);
        continue;
    }

    if (string.Equals(command, "stop-send-telemetry", StringComparison.OrdinalIgnoreCase))
    {
        telemetryState = await telemetryHandlers.HandleStopAsync(telemetryState, Console.Out);
        continue;
    }

    Console.WriteLine($"Unsupported command: {command}");
    Console.WriteLine("Supported commands: register-device, start-send-telemetry, stop-send-telemetry");
}
