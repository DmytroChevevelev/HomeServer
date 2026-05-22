namespace SmartHome.Api.Services;

public sealed class DeviceStatusEvaluator
{
    private static readonly TimeSpan ActiveWindow = TimeSpan.FromMinutes(5);

    public string Evaluate(DateTime? latestEventTimeUtc)
    {
        if (!latestEventTimeUtc.HasValue)
        {
            return "stale";
        }

        return latestEventTimeUtc.Value >= DateTime.UtcNow.Subtract(ActiveWindow) ? "active" : "stale";
    }
}
