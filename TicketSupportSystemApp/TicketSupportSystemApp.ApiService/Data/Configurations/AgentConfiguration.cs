using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketSupportSystemApp.ApiService.Data.Entities;

namespace TicketSupportSystemApp.ApiService.Data.Configurations;
public class AgentConfiguration : IEntityTypeConfiguration<Agent>
{
    public void Configure(EntityTypeBuilder<Agent> builder)
    {
        builder.Property(agent => agent.Name)
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(agent => agent.Surname)
            .HasMaxLength(100)
            .IsRequired();
        builder.HasMany(agent => agent.AssignedTickets)
            .WithOne(ticket => ticket.AssignedTo)
            .HasForeignKey(ticket => ticket.AssignedToId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(agent => agent.Comments)
                .WithOne(comment => comment.UserId)
                .HasForeignKey(comment => comment.AgentId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
    }
}
