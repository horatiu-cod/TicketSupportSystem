using Microsoft.EntityFrameworkCore;
using TicketSupportSystemApp.ApiService.Data;
using TicketSupportSystemApp.ApiService.Data.Enums;
using TicketSupportSystemApp.ApiService.DTOs.Requests;
using TicketSupportSystemApp.ApiService.DTOs.Responses;
using TicketSupportSystemApp.ApiService.Interfaces;
using TicketSupportSystemApp.ApiService.Common.Exceptions;

namespace TicketSupportSystemApp.ApiService.Services;
public class TicketsService(TicketSupportSystemContext context) : ITicketsService
{
    private readonly TicketSupportSystemContext _context = context;

    public async Task<Guid> CreateTicket(CreateTicketDTO ticketDTO)
    {
        var newTicket = Mappers.MapToTicket(ticketDTO);
        newTicket.SetStatus();

        _context.Tickets.Add(newTicket);
        await _context.SaveChangesAsync();

        return newTicket.Id;
    }

    public async Task DeleteTicket(Guid id)
    {
        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket == null)
        {
            throw new NotFoundException();
        }
        _context.Tickets.Remove(ticket);
        await _context.SaveChangesAsync();
    }

    public async Task<TicketDetailsDTO> GetTicket(Guid id)
    {
        var tickets = await _context.Tickets
            .Include(ticket => ticket.Customer)
            .Include(ticket => ticket.AssignedTo)
            .Include(ticket => ticket.Comments)
            .ToListAsync();
        var ticket = tickets.FirstOrDefault(t => t.Id == id) ?? throw new NotFoundException();
        var ticketDto = Mappers.MapToTicketDetailsDTO(ticket);

        return ticketDto;
    }

    private async Task<FilteredTicketsDTO> ApplyFilters(TicketsQueryFilters filters)
    {
        var query = _context.Tickets
            .Include(ticket => ticket.Customer)
            .Include(ticket => ticket.AssignedTo)
            .Include(ticket => ticket.Comments)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filters.SortColumn) && !string.IsNullOrWhiteSpace(filters.Order))
        {
            if (filters.Order == "asc")
            {
                query = query.OrderBy(e => EF.Property<object>(e, filters.SortColumn));
            }
            else if (filters.Order == "desc")
            {
                query = query.OrderByDescending(e => EF.Property<object>(e, filters.SortColumn));
            }
        }

        if(filters.Priority != null)
        {
            query = query.Where(t => t.Priority == filters.Priority);
        }

        if (filters.Status != null)
        {
            query = query.Where(t => t.Status == filters.Status);
        }

        if (filters.CustomerId != null)
        {
            query = query.Where(t => t.CustomerId == filters.CustomerId);
        }

        if (filters.AssignedToId != null)
        {
            query = query.Where(t => t.AssignedToId == filters.AssignedToId);
        }

        var total = await query.CountAsync();
        var take = filters.Take;
        var skip = filters.Skip;

        var tickets = await query
            .Skip(skip)
            .Take(take)
            .ToListAsync();

        var ticketsDTOs = tickets.Select(Mappers.MapToTicketDTO).ToList();


        var filteredTickets = new FilteredTicketsDTO(take, skip, total)
        {
            Tickets = ticketsDTOs
        };

        return filteredTickets;
    } 

    public async Task<FilteredTicketsDTO> GetTickets(TicketsQueryFilters filters)
    {
        var filteredTickets = await ApplyFilters(filters);

        return filteredTickets;
    }

    public async Task UpdateTicket(Guid id, UpdateTicketDTO ticketDTO)
    {
        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket == null)
        {
            throw new NotFoundException();
        }

        if (ticket.Status == Status.Closed)
        {
            throw new ForbiddenException();
        }
        ticket.Update(ticketDTO.Title, ticketDTO.Description, ticketDTO.Priority, ticketDTO.Status, 
                    ticketDTO.AssignedToId);

        await _context.SaveChangesAsync();
    }
}
