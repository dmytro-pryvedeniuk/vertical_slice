using MediatR.Pipeline;
using TravelInspiration.API.Shared.Security;

namespace TravelInspiration.API.Shared.Behaviors;

public sealed class LoggingBehavior<TRequest>(
    ICurrentUserService currentUserService,
    ILogger<TRequest> logger)
    : IRequestPreProcessor<TRequest>
    where TRequest : notnull
{
    private readonly ILogger<TRequest> _logger = logger;

    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Started feature execution: {FeatureName}, userId: {UserId}",
            typeof(TRequest).Name, currentUserService.UserId);

        return Task.CompletedTask;
    }
}
