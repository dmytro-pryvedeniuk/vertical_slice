namespace TravelInspiration.API.Shared.Domain.Entities;

public abstract class AuditableEntity
{
    public DateTime CreatedOn { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? LastModifiedAt { get; set; }
    public string? LastModifiedBy { get; set; }
}
