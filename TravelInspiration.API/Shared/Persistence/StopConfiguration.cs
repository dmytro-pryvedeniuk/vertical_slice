using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelInspiration.API.Shared.Domain.Entities;

namespace TravelInspiration.API.Shared.Persistence;

public class StopConfiguration : IEntityTypeConfiguration<Stop>
{
    public void Configure(EntityTypeBuilder<Stop> builder)
    {
        builder.ToTable("Stops");
        builder.Property(s => s.Id).UseIdentityColumn();
        builder.Property(s => s.Name).IsRequired().HasMaxLength(200);
        builder.Ignore(s => s.DomainEvents);
        builder.Property(s => s.Suggested)
            .HasDefaultValue(false);
    }
}
