using MediatR;

namespace TravelInspiration.API.Shared.Domain;

public abstract class DomainEvent : INotification
{
    public DateTimeOffset OccurredOn { get; set; } = DateTimeOffset.UtcNow;
}
