using SmartHome.Simulator.Runtime;
using Xunit;

namespace SmartHome.Simulator.Tests.Telemetry;

public sealed class TelemetrySessionStateTests
{
    [Fact]
    public void CanStart_ReturnsTrue_WhenIdle()
    {
        var allowed = TelemetrySessionStateGuard.CanStart(TelemetrySessionState.Idle, out var reason);

        Assert.True(allowed);
        Assert.Equal(string.Empty, reason);
    }

    [Fact]
    public void CanStart_ReturnsFalse_WhenAlreadySending()
    {
        var allowed = TelemetrySessionStateGuard.CanStart(TelemetrySessionState.Sending, out var reason);

        Assert.False(allowed);
        Assert.Contains("already", reason, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void CanStop_ReturnsTrue_WhenSending()
    {
        var allowed = TelemetrySessionStateGuard.CanStop(TelemetrySessionState.Sending, out var reason);

        Assert.True(allowed);
        Assert.Equal(string.Empty, reason);
    }

    [Fact]
    public void CanStop_ReturnsFalse_WhenIdle()
    {
        var allowed = TelemetrySessionStateGuard.CanStop(TelemetrySessionState.Idle, out var reason);

        Assert.False(allowed);
        Assert.Contains("No active", reason, StringComparison.OrdinalIgnoreCase);
    }
}