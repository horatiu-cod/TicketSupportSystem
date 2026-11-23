namespace TicketSupportSystemApp.ApiService.Data.Entities;

public sealed class Agent
{
    Guid Id {get; private set;}
    
    public List<string> Skills {get; private set;} = [];
    public List<Ticket> AssignedTickets { get; set; } = [];
    public AgentDetails? AgentDetails {get; private set;}

}