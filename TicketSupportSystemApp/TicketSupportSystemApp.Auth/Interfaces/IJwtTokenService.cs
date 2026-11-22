using TicketSupportSystemApp.Auth.Data.Entities;

namespace TicketSupportSystemApp.Auth.Interfaces;

public interface IJwtTokenService
{
    public Task<string> GenerateJwtToken(User user);
}