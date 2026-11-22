using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketSupportSystemApp.ApiService.Data.Entities;

namespace TicketSupportSystemApp.ApiService.Data.Configurations;
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasDiscriminator<string>("UserType")
        .HasValue<Customer>("Customer")
        .HasValue<Agent>("Agent");
        builder.Property(user => user.Name)
                .HasMaxLength(100)
                .IsRequired();
        builder.Property(user => user.Surname)
                .HasMaxLength(100)
                .IsRequired();
        builder.HasMany(user => user.Comments)
                .WithOne(comment => comment.User)
                .HasForeignKey(comment => comment.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
    }
}