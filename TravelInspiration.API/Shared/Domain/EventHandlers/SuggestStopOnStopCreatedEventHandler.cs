﻿using MediatR;
using TravelInspiration.API.Shared.Domain.Entities;
using TravelInspiration.API.Shared.Domain.Events;
using TravelInspiration.API.Shared.Persistence;

namespace TravelInspiration.API.Shared.Domain.EventHandlers;

public sealed class SuggestStopOnStopCreatedEventHandler
    (ILogger<SuggestStopOnStopCreatedEventHandler> logger,
    TravelInspirationDbContext dbContext)
    : INotificationHandler<StopCreatedEvent>
{
    private readonly ILogger<SuggestStopOnStopCreatedEventHandler> _logger = logger;
    private readonly TravelInspirationDbContext _dbContext = dbContext;

    public Task Handle(
        StopCreatedEvent notification,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Listener {Listener} to domain event {DomainEvent} triggered.",
            GetType().Name,
            notification.GetType().Name);

        var stop = new Stop($"AI-generated stop based on {notification.Name}")
        {
            ItineraryId = notification.ItineraryId,
            ImageUri = new Uri("http://herebeimages.com/aigeneratedimage.png"),
            Suggested = true
        };

        _dbContext.Stops.Add(stop);
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
