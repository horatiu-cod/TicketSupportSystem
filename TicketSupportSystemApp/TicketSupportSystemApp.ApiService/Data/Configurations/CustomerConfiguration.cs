using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketSupportSystemApp.ApiService.Data.Entities;

namespace TicketSupportSystemApp.ApiService.Data.Configurations;
public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        // Configure the primary key for the Customer entity
        builder.HasKey(c => c.Id);

        // Configure the relationship between Customer and Ticket
        builder.HasMany(customer => customer.CreatedTickets)
                .WithOne(ticket => ticket.Customer)
                .HasForeignKey(ticket => ticket.CustomerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
    }
}
