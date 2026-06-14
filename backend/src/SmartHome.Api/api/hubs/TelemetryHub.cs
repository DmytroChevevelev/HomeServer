using Microsoft.AspNetCore.SignalR;

namespace SmartHome.Api.Api.Hubs;

/// <summary>
/// SignalR hub used to publish telemetry updates to connected frontend clients.
/// </summary>
public sealed class TelemetryHub : Hub
{
    /// <summary>
    /// Event name emitted when a device latest sensor value changes.
    /// </summary>
    public const string SensorValueChangedEventName = "sensorValueChanged";
}
