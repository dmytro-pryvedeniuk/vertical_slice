using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TravelInspiration.API.Shared.Domain.Entities;
using TravelInspiration.API.Shared.Security;

namespace TravelInspiration.API.Shared.Persistence;

public sealed class UpdateAuditableEntitiesInterceptor(ICurrentUserService currentUserService)
    : SaveChangesInterceptor
{
    private readonly ICurrentUserService _currentUserService = currentUserService;

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
            UpdateAuditableEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateAuditableEntities(DbContext dbContext)
    {
        var userId = _currentUserService.UserId ?? string.Empty;
        foreach (var entry in dbContext.ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = userId;
                    entry.Entity.CreatedOn = DateTime.UtcNow;
                    entry.Entity.LastModifiedBy = userId;
                    entry.Entity.LastModifiedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.LastModifiedBy = userId;
                    entry.Entity.LastModifiedAt = DateTime.UtcNow;
                    break;
            }
        }
    }
}
