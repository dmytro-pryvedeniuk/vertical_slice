namespace TravelInspiration.API.Shared.Domain;

public interface IHasDomainEvents
{
    public IList<DomainEvent> DomainEvents { get; }
}
