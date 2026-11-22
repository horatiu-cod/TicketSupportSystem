using FluentValidation;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TicketSupportSystemApp.ApiService.Common.Exceptions;
using TicketSupportSystemApp.ApiService.Data.Entities;
using TicketSupportSystemApp.ApiService.DTOs.Requests;
using TicketSupportSystemApp.ApiService.Interfaces;

namespace TicketSupportSystemApp.ApiService;

public static class Endpoints
{
    public static void MapEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGroup("/api/comments/")
            .MapCommentsEndpoints()
            .WithTags("Comments");
        routes.MapGroup("/api/attachments/")
            .MapAttachementsEndpoints()
            .WithTags("Attachments");
        routes.MapGroup("/api/tickets/")
            .MapTicketsEndpoints()
            .WithTags("Tickets");
    }
    public static RouteGroupBuilder MapCommentsEndpoints(this RouteGroupBuilder group)
    {
        group.MapPut("{id}", async (Guid id, UpdateCommentDTO commentDTO, 
            ICommentsService _commentsService, IValidator<UpdateCommentDTO> updateCommentValidator,
            UserManager<User> userManager , ClaimsPrincipal user) =>
        {
            var validationRes = updateCommentValidator.Validate(commentDTO);
            if (!validationRes.IsValid)
                return Results.BadRequest(validationRes);

            try
            {
                var currentUserEmail = user.FindFirstValue(ClaimTypes.NameIdentifier)?? throw new NotFoundException("User email not found in claims.");

                var currentUser = await userManager.FindByEmailAsync(currentUserEmail)?? throw new NotFoundException("User not found.");
                var isCustomer = await userManager.IsInRoleAsync(currentUser, "Customer");

                if (isCustomer && currentUser.Comments.FirstOrDefault(c => c.Id == id) == null)
                {
                    return Results.Forbid();
                }

                await _commentsService.UpdateComment(id, commentDTO);

                return Results.Ok();
            }
            catch (NotFoundException ex)
            {
                return Results.NotFound(ex.Message);
            }
        }).RequireAuthorization( policy => policy.RequireRole("Admin"));

        group.MapDelete("{id}", async (Guid id, ICommentsService _commentsService) =>
        {
            try
            {
                await _commentsService.DeleteComment(id);

                return Results.Ok();
            }
            catch (NotFoundException)
            {
                return Results.NotFound();
            }
        }).RequireAuthorization();

        return group;
    }
    public static RouteGroupBuilder MapAttachementsEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost("{commentId}", async (Guid commentId , IFormFile file, IFileService _fileService)=>
        {
            try
            {
                var attachment = await _fileService.SaveAttachment(file, commentId);
                return Results.Ok(attachment);
            } 
            catch(NotFoundException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        });
        group.MapGet("{commentId}", async (Guid commentId, IFileService _fileService) =>
        {
            try
            {
                var attachments = await _fileService.GetAttachmentsToComment(commentId);
                return Results.Ok(attachments);
            }
            catch (NotFoundException ex)
            {
                return Results.NotFound(ex.Message);
            }
        });
        group.MapDelete("{attachmentId}", async (Guid attachmentId, IFileService _fileService) =>
        {
            try
            {
                await _fileService.DeleteAttachment(attachmentId);
                return Results.NoContent();
            }
            catch (NotFoundException ex)
            {
                return Results.NotFound(ex.Message);
            }
        });
        return group;
    }
    public static RouteGroupBuilder MapTicketsEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/", async (TicketsQueryFilters filters, ITicketsService _ticketsService) =>
        {
            var tickets = await _ticketsService.GetTickets(filters);
            return Results.Ok(tickets);
        }).RequireAuthorization(policy => policy.RequireRole("Admin", "SupportAgent"));

        group.MapGet("{id}", async (Guid id, ITicketsService _ticketsService, UserManager<User> _userManager, ClaimsPrincipal user) =>
        {
           try
            {
                var ticket = await _ticketsService.GetTicket(id);

                var currentUserEmail = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var curentUser = await _userManager.FindByEmailAsync(currentUserEmail?? throw new NotFoundException("User email not found in claims.")) 
                    ?? throw new NotFoundException("User not found.");
                var isCustomer = await _userManager.IsInRoleAsync(curentUser, "Customer");

                if (isCustomer && ticket.CustomerId != curentUser.Id)
                {
                    return Results.Forbid();
                }

                return Results.Ok(ticket);
            }
            catch (NotFoundException)
            {
                return Results.NotFound();
            }        
        }).RequireAuthorization();

        group.MapPost("/", async (CreateTicketDTO ticketDTO, ITicketsService _ticketsService, IValidator<CreateTicketDTO> createTicketValidator) =>
        {
            var validationRes = createTicketValidator.Validate(ticketDTO);
            if (!validationRes.IsValid)
                return Results.BadRequest(validationRes);

            var ticketId = await _ticketsService.CreateTicket(ticketDTO);
            return Results.Created($"/api/tickets/{ticketId}", new { Id = ticketId });
        }).RequireAuthorization();

        group.MapPut("{id}", async (Guid id, UpdateTicketDTO ticketDTO, ITicketsService _ticketsService) =>
        {
            try
            {
                await _ticketsService.UpdateTicket(id, ticketDTO);
                return Results.Ok();
            }
            catch (NotFoundException)
            {
                return Results.NotFound();
            }
        }).RequireAuthorization();

        group.MapDelete("{id}", async (Guid id, ITicketsService _ticketsService) =>
        {
            try
            {
                await _ticketsService.DeleteTicket(id);
                return Results.Ok();
            }
            catch (NotFoundException)
            {
                return Results.NotFound();
            }
        }).RequireAuthorization(policy => policy.RequireRole("Admin"));

        group.MapPost("{id}/comments", async (Guid id, CreateCommentDTO commentDTO, ICommentsService _commentsService, IValidator<CreateCommentDTO> createCommentValidator) =>
        {
            var validationRes = createCommentValidator.Validate(commentDTO);
            if (!validationRes.IsValid)
                return Results.BadRequest(validationRes);

            try
            {
                var commentId = await _commentsService.CreateComment(commentDTO);
                return Results.Created($"/api/comments/{commentId}", new { Id = commentId });
            }
            catch (NotFoundException)
            {
                return Results.NotFound();
            }
        }).RequireAuthorization();

        group.MapGet("{id}/comments", async (Guid id, ICommentsService _commentsService, ITicketsService _ticketsService,
                                            UserManager<User> _userManager, ClaimsPrincipal User) =>
        {
            var currentUserEmail = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userManager.FindByEmailAsync(currentUserEmail?? throw new NotFoundException("User email not found in claims.")) 
                ?? throw new NotFoundException("User not found.");
            var isCustomer = await _userManager.IsInRoleAsync(user, "Customer");
            var ticket = await _ticketsService.GetTicket(id) ?? throw new NotFoundException("Ticket not found.");

            if (isCustomer && ticket.CustomerId != user.Id)
            {
                return Results.Forbid();
            }

            try
            {
               var comments  = await _commentsService.GetCommentsToTicket(id);
               return Results.Ok(comments);
            }
            catch (NotFoundException)
            {
                return Results.NotFound();
            }
        }).RequireAuthorization();

        return group;
    }
}
