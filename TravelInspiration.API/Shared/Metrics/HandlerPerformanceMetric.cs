using System.Diagnostics.Metrics;

namespace TravelInspiration.API.Shared.Metrics;

public class HandlerPerformanceMetric
{
    private readonly Counter<long> _millisecondsElapsed;

    public HandlerPerformanceMetric(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create("TravelInspiration.API");
        _millisecondsElapsed = meter.CreateCounter<long>(
            "travelinspiration.api.requesthandler.millisecondselapsed");
    }

    public void MillisecondsElapsed(long millisecondsElapsed)
    {
        _millisecondsElapsed.Add(millisecondsElapsed);
    }
}
