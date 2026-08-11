using TicketSupportSystemApp.ApiService.Data.Enums;

namespace TicketSupportSystemApp.ApiService.DTOs.Requests;

public record CreateTicketDTO(string Title, string Description, Priority Priority, 
                            Guid UserId, Guid? AssignedToId);
