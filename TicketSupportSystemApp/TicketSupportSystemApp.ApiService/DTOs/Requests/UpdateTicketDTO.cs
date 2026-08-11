using TicketSupportSystemApp.ApiService.Data.Enums;
namespace TicketSupportSystemApp.ApiService.DTOs.Requests;

public record UpdateTicketDTO(string Title, string Description, Priority Priority, 
                                Status Status, Guid? AssignedToId);
