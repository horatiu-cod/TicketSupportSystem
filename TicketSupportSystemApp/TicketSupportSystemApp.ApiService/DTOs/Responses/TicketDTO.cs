using TicketSupportSystemApp.ApiService.Data.Enums;
namespace TicketSupportSystemApp.ApiService.DTOs.Responses;

public record TicketDTO(Guid Id, string Title, string Description, Priority Priority, 
                        Status Status, DateTimeOffset CreatedAt, DateTimeOffset? UpdatedAt, 
                        DateTimeOffset? ClosedAt, Guid UserId, string UserName, string UserEmail, 
                        string? AssignedToName, string? AssignedToEmail);
