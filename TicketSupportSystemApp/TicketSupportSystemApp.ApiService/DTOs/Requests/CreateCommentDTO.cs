namespace TicketSupportSystemApp.ApiService.DTOs.Requests;

public record CreateCommentDTO(string Text, Guid UserId, Guid TicketId);
