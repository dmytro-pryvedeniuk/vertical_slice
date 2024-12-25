namespace TravelInspiration.API.Shared.Domain.Events;

public class StopUpdatedEvent(int itineraryId, string name) : DomainEvent
{
    public int ItineraryId { get; } = itineraryId;
    public string Name { get; } = name;
}
