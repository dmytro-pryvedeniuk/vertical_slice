using MediatR;
using System.Diagnostics;
using TravelInspiration.API.Shared.Metrics;

namespace TravelInspiration.API.Shared.Behaviors;

public sealed class HandlerPerformanceMetricBehavior<TRequest, TResponse>
    (HandlerPerformanceMetric metric)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly HandlerPerformanceMetric _metric = metric;
    private readonly Stopwatch _stopwatch = new();

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _stopwatch.Start();
        var response = await next();
        _stopwatch.Stop();

        _metric.MillisecondsElapsed(_stopwatch.ElapsedMilliseconds);

        return response;
    }
}
