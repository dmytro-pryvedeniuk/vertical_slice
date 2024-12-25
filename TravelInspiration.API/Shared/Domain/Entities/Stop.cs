using TravelInspiration.API.Shared.Domain.Events;
using static TravelInspiration.API.Features.Stops.CreateStop;
using static TravelInspiration.API.Features.Stops.UpdateStop;

namespace TravelInspiration.API.Shared.Domain.Entities;

public sealed class Stop(string name) : AuditableEntity, IHasDomainEvents
{
    public int Id { get; set; }
    public string Name { get; set; } = name;
    public Uri? ImageUri { get; set; }
    public int ItineraryId { get; set; }
    public bool? Suggested { get; set; }
    public Itinerary? Itinerary { get; set; }

    public IList<DomainEvent> DomainEvents { get; } = [];

    public void HandleCreateStopCommand(CreateStopCommand request)
    {
        ItineraryId = request.ItineraryId;
        ImageUri = request.ImageUri is null ? null : new Uri(request.ImageUri);
        DomainEvents.Add(new StopCreatedEvent(ItineraryId, Name));
    }

    public void HandleUpdateStopCommand(UpdateStopCommand request)
    {
        Name = request.Name;
        ImageUri = request.ImageUri is null ? null : new Uri(request.ImageUri);
        Suggested = request.Suggested;
        DomainEvents.Add(new StopUpdatedEvent(ItineraryId, Name));
    }
}
