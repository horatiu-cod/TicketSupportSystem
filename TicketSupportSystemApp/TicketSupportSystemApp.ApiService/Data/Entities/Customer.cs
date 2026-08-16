namespace TicketSupportSystemApp.ApiService.Data.Entities;

public sealed class Customer : User
{
    public List<Ticket> Tickets { get; private set; } = [];

    private Customer()
    {
    }

}
