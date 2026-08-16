using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketSupportSystemApp.ApiService.Data.Entities;

namespace TicketSupportSystemApp.ApiService.Data.Configurations;
public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.Property(customer => customer.Name)
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(customer => customer.Surname)
            .HasMaxLength(100)
            .IsRequired();
        builder.HasMany(customer => customer.CreatedTickets)
                .WithOne(ticket => ticket.Customer)
                .HasForeignKey(ticket => ticket.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(customer => customer.Comments)
                .WithOne(comment => comment.Customer)
                .HasForeignKey(comment => comment.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
    }
}
