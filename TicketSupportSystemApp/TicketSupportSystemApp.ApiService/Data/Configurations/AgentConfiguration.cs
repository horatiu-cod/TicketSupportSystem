using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketSupportSystemApp.ApiService.Data.Entities;

namespace TicketSupportSystemApp.ApiService.Data.Configurations;
public class AgentConfiguration : IEntityTypeConfiguration<Agent>
{
    public void Configure(EntityTypeBuilder<Agent> builder)
    {
        builder.HasKey(c => c.Id);

        builder.HasMany(agent => agent.AssignedTickets)
            .WithOne(ticket => ticket.AssignedTo)
            .HasForeignKey(ticket => ticket.AssignedToId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
