using SmartHome.Simulator.Runtime;

namespace SmartHome.Simulator.Commands;

public sealed class TelemetryControlCommandHandlers(
    TelemetryPublisher publisher,
    TelemetryProfileLoader profileLoader)
{
    public async Task<TelemetrySessionState> HandleStartAsync(
        SimulatorDeviceProfile profile,
        TelemetrySessionState currentState,
        TextWriter output,
        CancellationToken appCt)
    {
        if (!TelemetrySessionStateGuard.CanStart(currentState, out var reason))
        {
            await output.WriteLineAsync($"start-send-telemetry: {reason}");
            return currentState;
        }

        TelemetryProfile telemetryProfile;
        try
        {
            telemetryProfile = profileLoader.LoadForDevice(profile);
        }
        catch (Exception ex)
        {
            await output.WriteLineAsync($"start-send-telemetry: profile error ({ex.Message})");
            return TelemetrySessionState.Faulted;
        }

        var started = await publisher.StartAsync(profile, telemetryProfile, output, appCt);
        if (!started)
        {
            await output.WriteLineAsync("start-send-telemetry: Telemetry is already running.");
            return TelemetrySessionState.Sending;
        }

        return TelemetrySessionState.Sending;
    }

    public async Task<TelemetrySessionState> HandleStopAsync(
        TelemetrySessionState currentState,
        TextWriter output)
    {
        if (!TelemetrySessionStateGuard.CanStop(currentState, out var reason))
        {
            await output.WriteLineAsync($"stop-send-telemetry: {reason}");
            return currentState;
        }

        var stopped = await publisher.StopAsync(output);
        if (!stopped)
        {
            await output.WriteLineAsync("stop-send-telemetry: No active telemetry loop to stop.");
            return TelemetrySessionState.Idle;
        }

        return TelemetrySessionState.Idle;
    }
}