namespace SmartHome.Simulator.Runtime;

public enum TelemetrySessionState
{
    Idle = 0,
    Sending = 1,
    Faulted = 2
}

public static class TelemetrySessionStateGuard
{
    public static bool CanStart(TelemetrySessionState state, out string reason)
    {
        if (state == TelemetrySessionState.Sending)
        {
            reason = "Telemetry is already running.";
            return false;
        }

        reason = string.Empty;
        return true;
    }

    public static bool CanStop(TelemetrySessionState state, out string reason)
    {
        if (state != TelemetrySessionState.Sending)
        {
            reason = "No active telemetry loop to stop.";
            return false;
        }

        reason = string.Empty;
        return true;
    }
}