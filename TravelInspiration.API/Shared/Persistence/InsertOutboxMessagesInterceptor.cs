using Microsoft.EntityFrameworkCore.Diagnostics;
using TravelInspiration.API.Shared.Domain;
using TravelInspiration.API.Shared.Domain.Entities;

namespace TravelInspiration.API.Shared.Persistence;

public sealed class InsertOutboxMessagesInterceptor()
    : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is TravelInspirationDbContext context)
            InsertOutboxMessages(context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void InsertOutboxMessages(TravelInspirationDbContext context)
    {
        var entriesWithDomainEvents = context.ChangeTracker.Entries<IHasDomainEvents>().ToArray();
        var outboxMessages = entriesWithDomainEvents
            .SelectMany(x => x.Entity.DomainEvents)
            .Select(x => new OutboxMessage
            {
                OccurredOn = x.OccurredOn,
                Content = DomainEventSerializer.Serialize(x)
            });

        context.OutboxMessages.AddRange(outboxMessages);
        foreach (var entry in entriesWithDomainEvents)
            entry.Entity.DomainEvents.Clear();
    }
}
