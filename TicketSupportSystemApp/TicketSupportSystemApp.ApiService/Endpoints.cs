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

        group.MapDelete("/{id}", async (Guid id, ICommentsService _commentsService) =>
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
}
