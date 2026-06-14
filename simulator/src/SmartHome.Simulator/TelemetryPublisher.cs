using SmartHome.Simulator.Runtime;

namespace SmartHome.Simulator;

public sealed class TelemetryPublisher(SimulatorBackendClient backendClient)
{
    private readonly object _gate = new();
    private CancellationTokenSource? _sendCts;
    private Task? _sendTask;

    public bool IsRunning
    {
        get
        {
            lock (_gate)
            {
                return _sendTask is { IsCompleted: false };
            }
        }
    }

    public async Task<bool> StartAsync(
        SimulatorDeviceProfile profile,
        TelemetryProfile telemetryProfile,
        TextWriter output,
        CancellationToken appCt)
    {
        lock (_gate)
        {
            if (_sendTask is { IsCompleted: false })
            {
                return false;
            }

            _sendCts = CancellationTokenSource.CreateLinkedTokenSource(appCt);
            _sendTask = RunLoopAsync(profile, telemetryProfile, output, _sendCts.Token);
        }

        await output.WriteLineAsync($"start-send-telemetry: started with interval {profile.SendIntervalSeconds}s");
        return true;
    }

    public async Task<bool> StopAsync(TextWriter output)
    {
        Task? taskToAwait;
        lock (_gate)
        {
            if (_sendTask is null || _sendTask.IsCompleted)
            {
                return false;
            }

            _sendCts?.Cancel();
            taskToAwait = _sendTask;
        }

        try
        {
            await taskToAwait.ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            // Expected during stop.
        }

        await output.WriteLineAsync("stop-send-telemetry: stopped");
        return true;
    }

    private async Task RunLoopAsync(
        SimulatorDeviceProfile profile,
        TelemetryProfile telemetryProfile,
        TextWriter output,
        CancellationToken ct)
    {
        var index = 0;
        while (!ct.IsCancellationRequested)
        {
            var metric = telemetryProfile.Metrics[index % telemetryProfile.Metrics.Count];
            index++;

            using var response = await backendClient.SendTelemetryAsync(profile, metric, ct);
            var body = await response.Content.ReadAsStringAsync(ct);
            await output.WriteLineAsync($"telemetry: {(int)response.StatusCode} {body}");

            await Task.Delay(TimeSpan.FromSeconds(profile.SendIntervalSeconds), ct);
        }
    }
}
