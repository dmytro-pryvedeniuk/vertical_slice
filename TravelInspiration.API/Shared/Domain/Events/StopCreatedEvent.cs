namespace TravelInspiration.API.Shared.Domain.Events;

public class StopCreatedEvent(int itineraryId, string name) : DomainEvent
{
    public int ItineraryId { get; } = itineraryId;
    public string Name { get; } = name;
}
