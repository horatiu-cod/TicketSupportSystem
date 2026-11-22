using Microsoft.AspNetCore.Identity;
using TicketSupportSystemApp.Auth.Data.Entities;
using TicketSupportSystemApp.Auth.DTOs;
using TicketSupportSystemApp.Auth.Interfaces;
using TicketSupportSystemApp.Auth.Validators;

namespace TicketSupportSystemApp.Auth;

public static class AuthEndpoints
{
    public static void MapEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGroup("/api/auth/")
            .MapAuthEndpoints()
            .WithTags("Authentication");
    }

    public static RouteGroupBuilder MapAuthEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost("login", async (LoginDTO loginDTO, IJwtTokenService jwtTokenService, UserManager<User> userManager, LoginValidator loginValidator) =>
        {
            var validationResult = await loginValidator.ValidateAsync(loginDTO);
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.Errors);
            }
            var user = await userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null || !await userManager.CheckPasswordAsync(user, loginDTO.Password))
            {
                return Results.Unauthorized();
            }

            var token = await jwtTokenService.GenerateJwtToken(user);
            return Results.Ok(new { Token = token });
        });

        group.MapPost("register", async (UserRegistrationDTO registrationDTO, UserManager<User> userManager, RegistrationValidator registrationValidator) =>
        {
            var validationResult = await registrationValidator.ValidateAsync(registrationDTO);
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.Errors);
            }

            var user = new User
            {
                Email = registrationDTO.Email,
                UserName = registrationDTO.Email,
                PhoneNumber = registrationDTO.PhoneNumber,
            };

            var result = await userManager.CreateAsync(user, registrationDTO.Password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Customer");
                return Results.Ok(user.Id);
            }
            return Results.BadRequest(result.Errors);
        });

        return group;
    }
}