namespace TicketSupportSystemApp.ApiService.Data.Entities;
public sealed class Comment : EntityBase
{
    public string Text { get; private set; } 
    public Guid UserId { get; private set; }
    public Guid TicketId { get; private set; }
    //public User User { get; private set; } = null!;
    public Ticket Ticket { get; private set; } = null!;

    public List<Attachment> Attachments { get; private set; } = [];

    // For EF Core
    private Comment()
    {
        Text = string.Empty;
    }
    private Comment(string text, Guid userId, Guid ticketId)
    {
        Text = text;
        UserId = userId;
        TicketId = ticketId;
    }
    public static Comment Create(string text, Guid userId, Guid ticketId)
    {
        ValidateInputs(text, userId, ticketId);
        return new Comment(text, userId, ticketId);
    }
    public void UpdateText(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentException("Comment text cannot be null or empty.", nameof(text));
        }
        Text = text;
        UpdateLastModified();
    }
    private static void ValidateInputs(string text, Guid userId, Guid ticketId)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentException("Comment text cannot be null or empty.", nameof(text));
        }

        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User ID cannot be empty.", nameof(userId));
        }

        if (ticketId == Guid.Empty)
        {
            throw new ArgumentException("Ticket ID cannot be empty.", nameof(ticketId));
        }
    }
}
