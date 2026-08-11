using TicketSupportSystemApp.ApiService.DTOs.Requests;
using TicketSupportSystemApp.ApiService.DTOs.Responses;
namespace TicketSupportSystemApp.ApiService.Interfaces;

public interface ITicketsService
{
    public Task<Guid> CreateTicket(CreateTicketDTO ticketDTO);
    public Task UpdateTicket(Guid id, UpdateTicketDTO ticketDTO);
    public Task DeleteTicket(Guid id);
    public Task<FilteredTicketsDTO> GetTickets(TicketsQueryFilters filters);
    public Task<TicketDetailsDTO> GetTicket(Guid id);
}

