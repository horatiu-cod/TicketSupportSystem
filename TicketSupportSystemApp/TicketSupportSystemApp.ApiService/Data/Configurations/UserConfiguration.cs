using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketSupportSystemApp.ApiService.Data.Entities;

namespace TicketSupportSystemApp.ApiService.Data.Configurations;
public class UserConfiguration : IEntityTypeConfiguration<User>{}