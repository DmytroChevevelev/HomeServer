namespace SmartHome.Api.Models;

public class Device
{
    public Guid Id { get; set; }
    public string ExternalId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string SensorType { get; set; } = string.Empty;
    public DateTime RegisteredAtUtc { get; set; }
    public bool IsEnabled { get; set; } = true;
    public List<TelemetryReading> Readings { get; set; } = new();
}
