namespace TicketSupportSystemApp.ApiService.Data.Entities;
public abstract class EntityBase
{
    public Guid Id { get; private init; } = Guid.CreateVersion7();
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; private set; } = DateTimeOffset.UtcNow;
    public void UpdateLastModified()
    {
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}