using TicketSupportSystemApp.ApiService.Data.Enums;
namespace TicketSupportSystemApp.ApiService.Data.Entities;

public sealed class Ticket : EntityBase
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public Priority Priority { get; private set; }
    public Status Status { get; private set; }
    public DateTimeOffset? ClosedAt { get; private set; }
    public Guid CustomerId { get; private set; }
    public Guid? AssignedToId { get; private set; }
    public Customer Customer { get; private set; } = null!;
    public Agent? AssignedTo { get; private set; }
    public ICollection<Comment> Comments { get; private set; } = [];

    private Ticket()
    {
        Title = string.Empty;
        Description = string.Empty;
    }

    public Ticket(string title, string description, Priority priority, Guid customerId, Guid? 
                    assignedToId = null)
    {
        Title = title;
        Description = description;
        Priority = priority;
        CustomerId = customerId;
        AssignedToId = assignedToId;
    }

    public static Ticket Create(string title, string description, Priority priority, Guid customerId, 
                                Guid? assignedToId = null)
    {
        ValidateInputs(title, description, customerId);
        return new Ticket(title, description, priority, customerId, assignedToId);
    } 
    public void Update(string title, string description, Priority priority, Status status, Guid? assignedToId = null)
    {
        ValidateInputs(title, description);
        Title = title;
        Description = description;
        Priority = priority;
        Status = status;
        AssignedToId = assignedToId;

        if (status == Status.Closed)
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

    private static void ValidateInputs(string title, string description, Guid customerId = default(Guid))
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title cannot be null or empty.", nameof(title));
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("Description cannot be null or empty.", nameof(description));
        }

        if (customerId == Guid.Empty)
        {
            throw new ArgumentException("Customer ID cannot be empty.", nameof(customerId));
        }
    }

}
