using MediatR;
using Microsoft.EntityFrameworkCore;
using TravelInspiration.API.Shared.Domain;
using TravelInspiration.API.Shared.Domain.Entities;
using TravelInspiration.API.Shared.Security;

namespace TravelInspiration.API.Shared.Persistence;

public sealed class TravelInspirationDbContext(
    ICurrentUserService currentUserService,
    IPublisher publisher,
    DbContextOptions<TravelInspirationDbContext> options)
    : DbContext(options)
{
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IPublisher _publisher = publisher;

    public DbSet<Itinerary> Itineraries => Set<Itinerary>();
    public DbSet<Stop> Stops => Set<Stop>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        SeedTables(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(TravelInspirationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    private static void SeedTables(ModelBuilder modelBuilder)
    {
        var itineraries = new[] {
            new Itinerary("European Adventure", "user1")
            {
                Id = 1,
                Description = "A wonderful journey through Europe"
            },
            new Itinerary("Asia Exploration", "user2")
            {
                Id = 2,
                Description = "Exploring the wonders of Asia"
            }
        };
        var stops = new[] {
            new Stop("Paris")
            {
                Id = 1,
                ItineraryId = 1,
                ImageUri = new Uri("https://localhost:7120/paris.jpg")
            },
            new Stop("Rome")
            {
                Id = 2,
                ItineraryId = 1,
                ImageUri = new Uri("https://localhost:7120/rome.jpg")
            },
            new Stop("Bangkok")
            {
                Id = 3,
                ItineraryId = 2,
                ImageUri = new Uri("https://localhost:7120/bangkok.jpg")
            },
            new Stop("Tokyo")
            {
                Id = 4,
                ItineraryId = 2,
                ImageUri = new Uri("https://localhost:7120/tokyo.jpg")
            }
        };
        foreach (var entry in itineraries.Union<AuditableEntity>(stops))
        {
            entry.CreatedBy = "DATASEED";
            entry.CreatedOn = DateTime.Parse("2024-11-26 12:00:00");
        }
        modelBuilder.Entity<Itinerary>().HasData(itineraries);
        modelBuilder.Entity<Stop>().HasData(stops);
    }

    public override async Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId ?? string.Empty;
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
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

        var domainEvents = ChangeTracker.Entries<IHasDomainEvents>()
            .SelectMany(x => x.Entity.DomainEvents)
            .Where(x => !x.IsPublished)
            .ToArray();

        foreach (var domainEvent in domainEvents)
        {
            await _publisher.Publish(domainEvent, cancellationToken);
            domainEvent.IsPublished = true;
        }

        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}
