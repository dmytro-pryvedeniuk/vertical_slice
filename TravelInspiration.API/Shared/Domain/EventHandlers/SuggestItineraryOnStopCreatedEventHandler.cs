using MediatR;
using TravelInspiration.API.Shared.Domain.Events;
using TravelInspiration.API.Shared.Persistence;

namespace TravelInspiration.API.Shared.Domain.EventHandlers;

public sealed class SuggestItineraryOnStopCreatedEventHandler(
    ILogger<SuggestItineraryOnStopCreatedEventHandler> logger)
    : INotificationHandler<StopCreatedEvent>
{
    private readonly ILogger<SuggestItineraryOnStopCreatedEventHandler> _logger = logger;

    public Task Handle(
        StopCreatedEvent notification,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Listener {Listener} to domain event {DomainEvent} triggered.",
            GetType().Name,
            notification.GetType().Name);
        return Task.CompletedTask;
    }
}
