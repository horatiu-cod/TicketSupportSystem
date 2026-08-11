using TicketSupportSystemApp.ApiService.Data.Enums;
namespace TicketSupportSystemApp.ApiService.Data.Entities;

public sealed class Ticket : EntityBase
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public Priority Priority { get; private set; }
    public Status Status { get; private set; }
    public DateTimeOffset? ClosedAt { get; private set; }
    public Guid UserId { get; private set; }
    public Guid? AssignedToId { get; private set; }
    public User User { get; private set; } = null!;
    public User? AssignedTo { get; private set; }
    public List<Comment> Comments { get; private set; } = [];

    private Ticket()
    {
        Title = string.Empty;
        Description = string.Empty;
    }

    public Ticket(string title, string description, Priority priority, Guid userId, Guid? assignedToId = null)
    {
        Title = title;
        Description = description;
        Priority = priority;
        UserId = userId;
        AssignedToId = assignedToId;
    }

    public static Ticket Create(string title, string description, Priority priority, Guid userId, Guid? assignedToId = null)
    {
        ValidateInputs(title, description, userId);
        return new Ticket(title, description, priority, userId, assignedToId);
    } 
    public void Update(string newTitle, string newDescription, Priority newPriority, Status newStatus, Guid? newAssignedToId = null)
    {
        ValidateInputs(newTitle, newDescription, UserId);
        Title = newTitle;
        Description = newDescription;
        Priority = newPriority;
        Status = newStatus;
        AssignedToId = newAssignedToId;

        if (newStatus == Status.Closed)
        {
            ClosedAt = DateTimeOffset.UtcNow;
        }
        else
        {
            ClosedAt = null;
        }

        UpdateLastModified();
    }
    public void SetStatus()
    {
        Status = Status.Open;
        UpdateLastModified();
    }

    private static void ValidateInputs(string title, string description, Guid userId)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title cannot be null or empty.", nameof(title));
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("Description cannot be null or empty.", nameof(description));
        }

        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User ID cannot be empty.", nameof(userId));
        }
    }

}
