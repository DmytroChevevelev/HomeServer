using System;
using SmartHome.Api.Services;
using Xunit;

namespace SmartHome.Api.UnitTests.Devices;

public class DeviceStatusEvaluatorTests
{
    private readonly DeviceStatusEvaluator _sut = new();

    [Fact]
    public void NullTelemetry_Is_Stale()
    {
        Assert.Equal("stale", _sut.Evaluate(null));
    }

    [Fact]
    public void RecentTelemetry_Is_Active()
    {
        Assert.Equal("active", _sut.Evaluate(DateTime.UtcNow));
    }
}
