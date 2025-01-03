﻿using Microsoft.EntityFrameworkCore;
using TravelInspiration.API.Features.Stops;
using TravelInspiration.API.IntegrationTests.Fixtures;
using TravelInspiration.API.Shared.Domain.Entities;
using TravelInspiration.API.Shared.Domain.Events;
using TravelInspiration.API.Shared.Persistence;

namespace TravelInspiration.API.IntegrationTests.Features.Stops;

public sealed class CreateStopTests(SliceFixture sliceFixture)
    : IClassFixture<SliceFixture>
{
    private readonly SliceFixture sliceFixture = sliceFixture;

    [Fact]
    public async Task WhenExecutingCreateStopSlice_WithValidInput_StopMustBeCreatedAsync()
    {
        await sliceFixture.ExecuteInTransactionAsync(async (context) =>
        {
            var itinerary = new Itinerary("Test", "SomeUserId");
            context.Add(itinerary);
            await context.SaveChangesAsync();
            var cmd = new CreateStop.CreateStopCommand(itinerary.Id, "A stop for testing", null);

            // Act
            await sliceFixture.SendAsync(cmd);

            // Assert
            context.ChangeTracker.Clear();
            var stop = await context.Stops.FirstOrDefaultAsync(s => s.ItineraryId == itinerary.Id);
            Assert.NotNull(stop);
            Assert.Equal(cmd.Name, stop.Name);
        });
    }

    [Fact]
    public async Task WhenExecutingCreateStopSlice_WithValidInput_OutboxMessageForStopCreatedEventMustBeCreated()
    {
        await sliceFixture.ExecuteInTransactionAsync(async (context) =>
        {
            var itinerary = new Itinerary("Test", "SomeUserId");
            context.Add(itinerary);
            await context.SaveChangesAsync();
            var cmd = new CreateStop.CreateStopCommand(itinerary.Id, "A stop for testing", null);

            // Act
            await sliceFixture.SendAsync(cmd);

            // Assert
            context.ChangeTracker.Clear();
            var outboxMessage = await context.OutboxMessages.SingleAsync();
            var stopCreatedEvent = DomainEventSerializer.Deserialize<StopCreatedEvent>(outboxMessage.Content);
            Assert.Equal(cmd.ItineraryId, stopCreatedEvent.ItineraryId);
            Assert.Equal(cmd.Name, stopCreatedEvent.Name);
        });
    }
}
