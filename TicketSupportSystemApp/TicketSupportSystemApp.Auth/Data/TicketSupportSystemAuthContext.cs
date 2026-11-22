using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TicketSupportSystemApp.Auth.Data.Entities;
namespace TicketSupportSystemApp.Auth.Data;

public class TicketSupportSystemAuthContext(DbContextOptions<TicketSupportSystemAuthContext> options) : IdentityDbContext<User, Role, Guid>(options)
{

}

