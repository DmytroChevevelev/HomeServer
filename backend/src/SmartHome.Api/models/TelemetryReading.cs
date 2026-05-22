namespace SmartHome.Api.Models;

public class TelemetryReading
{
    public Guid Id { get; set; }
    public Guid DeviceId { get; set; }
    public Device? Device { get; set; }
    public string MetricType { get; set; } = string.Empty;
    public decimal MetricValue { get; set; }
    public DateTime EventTimeUtc { get; set; }
    public DateTime IngestedAtUtc { get; set; }
}
