using TicketSupportSystemApp.ApiService.Data.Enums;
namespace TicketSupportSystemApp.ApiService.DTOs.Responses;

public record TicketDetailsDTO(Guid Id, Priority Priority, Status Status, DateTimeOffset CreatedAt, 
                DateTimeOffset? UpdatedAt, DateTimeOffset? ClosedAt, Guid UserId, 
                string? AssignedToName, string? AssignedToEmail, 
                string Title, string Description, string UserName, string UserEmail)
{
    public List<CommentDTO> Comments { get; set; } = [];
}
