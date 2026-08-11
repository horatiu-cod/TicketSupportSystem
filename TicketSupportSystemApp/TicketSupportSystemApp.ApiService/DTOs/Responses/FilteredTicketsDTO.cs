namespace TicketSupportSystemApp.ApiService.DTOs.Responses;

public record FilteredTicketsDTO(int Take, int Skip, long Total)
{
    public List<TicketDTO> Tickets { get; set; } = [];
}
