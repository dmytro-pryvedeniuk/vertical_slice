using MediatR;
using TravelInspiration.API.Shared.Domain.Entities;
using TravelInspiration.API.Shared.Domain.Events;
using TravelInspiration.API.Shared.Persistence;

namespace TravelInspiration.API.Shared.Domain.EventHandlers;

public sealed class SuggestStopOnStopUpdatedEventHandler(
    ILogger<SuggestStopOnStopUpdatedEventHandler> logger,
    TravelInspirationDbContext dbContext)
    : INotificationHandler<StopUpdatedEvent>
{
    private readonly ILogger<SuggestStopOnStopUpdatedEventHandler> _logger = logger;
    private readonly TravelInspirationDbContext _dbContext = dbContext;

    public Task Handle(
        StopUpdatedEvent notification,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Listener {Listener} to domain event {DomainEvent} triggered.",
            GetType().Name,
            notification.GetType().Name);

        var incomingStop = notification.Stop;

        var stop = new Stop($"AI-generated stop based on {incomingStop.Name}")
        {
            ItineraryId = incomingStop.ItineraryId,
            ImageUri = new Uri("http://herebeimages.com/aigeneratedimage.png"),
            Suggested = true
        };

        _dbContext.Stops.Add(stop);
        return Task.CompletedTask;
    }
}
