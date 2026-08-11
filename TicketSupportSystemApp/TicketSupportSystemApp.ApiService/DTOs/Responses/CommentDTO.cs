namespace TicketSupportSystemApp.ApiService.DTOs.Responses;

public record CommentDTO(Guid Id, DateTimeOffset CreatedAt, DateTimeOffset? UpdatedAt, 
                        Guid UserId, Guid TicketId, string Text, string UserEmail, string TicketTitle);
