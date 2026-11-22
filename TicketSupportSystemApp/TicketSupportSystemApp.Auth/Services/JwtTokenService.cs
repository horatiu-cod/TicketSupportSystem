using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using TicketSupportSystemApp.Auth.Data.Entities;
using TicketSupportSystemApp.Auth.Interfaces;

namespace TicketSupportSystemApp.Auth.Services;

 public class JwtTokenService(IConfiguration configuration, UserManager<User> userManager) : IJwtTokenService
{
    private readonly IConfiguration _configuration = configuration;
    private readonly UserManager<User> _userManager = userManager;

    public async Task<string> GenerateJwtToken(User user)
    {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaims(user);
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

    private SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:SigningKey"] ?? throw new InvalidOperationException("Signing key not found in configuration."));
        var secret = new SymmetricSecurityKey(key);
        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var tokenOptions = new JwtSecurityToken
        (
        issuer: _configuration["Jwt:Issuer"],
        audience: _configuration["Jwt:Audience"],
        claims: claims,
        expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiresIn"])),
        signingCredentials: signingCredentials
        );
        return tokenOptions;
    }

    private async Task<List<Claim>> GetClaims(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Email?? throw new InvalidOperationException("User email not found.")),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        return claims;
    }
}
