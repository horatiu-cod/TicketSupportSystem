using TicketSupportSystemApp.ApiService.DTOs.Requests;
using TicketSupportSystemApp.ApiService.Data.Entities;
using TicketSupportSystemApp.ApiService.DTOs.Responses;

internal static class Mappers
{
    internal static Ticket MapToTicket(this CreateTicketDTO createTicketDTO)
    {
        return Ticket.Create(createTicketDTO.Title, createTicketDTO.Description, createTicketDTO.Priority, 
                            createTicketDTO.CustomerId);
    }
    internal static TicketDTO MapToTicketDTO(this Ticket ticket)
    {
        return new TicketDTO(ticket.Id, ticket.Title, ticket.Description, ticket.Priority, 
        ticket.Status, ticket.CreatedAt, ticket.UpdatedAt, ticket.ClosedAt, ticket.CustomerId, 
        ticket.Customer.Name, ticket.Customer.Email!, ticket.AssignedTo?.Name, ticket.AssignedTo?.Email);
    }
    internal static TicketDetailsDTO MapToTicketDetailsDTO(this Ticket ticket)
    {
        return new TicketDetailsDTO(ticket.Id, ticket.Priority, ticket.Status, 
        ticket.CreatedAt, ticket.UpdatedAt, ticket.ClosedAt, ticket.CustomerId, ticket.AssignedToId,
        ticket.AssignedTo?.Name, ticket.AssignedTo?.Email, ticket.Title, ticket.Description, 
        ticket.Customer.Name, ticket.Customer.Email!)
        {
            Comments = [.. ticket.Comments.Select(c => c.MapToCommentDTO())]
        };
    }
    internal static CommentDTO MapToCommentDTO(this Comment comment)
    {
        return new CommentDTO(comment.Id, comment.CreatedAt, comment.UpdatedAt, 
        comment.UserId, comment.TicketId, comment.Text, comment.User.Email!, comment.Ticket.Title);
    }
    internal static Comment MapToComment(this CreateCommentDTO createCommentDTO)
    {
        return Comment.Create(createCommentDTO.Text, createCommentDTO.UserId, 
        createCommentDTO.TicketId);
    }
    internal static UserDTO MapToUserDTO(this User user)
    {
        return new UserDTO(user.Id, user.Name, user.Email!);
    }
    internal static User MapToUser(this UserRegistrationDTO userRegistrationDTO)    
    {
        return new User
        {
            Name = userRegistrationDTO.Name,
            Email = userRegistrationDTO.Email,
        };
    }
    internal static AttachmentDTO MapToAttachmentDTO(this Attachment attachment)
    {
        return new AttachmentDTO(attachment.Id, attachment.FileName, attachment.Path);
    }
}

