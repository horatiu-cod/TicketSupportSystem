using Microsoft.EntityFrameworkCore;
using TicketSupportSystemApp.ApiService.Data;
using TicketSupportSystemApp.ApiService.DTOs.Requests;
using TicketSupportSystemApp.ApiService.DTOs.Responses;
using TicketSupportSystemApp.ApiService.Interfaces;
using TicketSupportSystemApp.ApiService.Common.Exceptions;

namespace TicketSupportSystemApp.ApiService.Services;

public class CommentsService : ICommentsService
{
    private readonly TicketSupportSystemContext _context;

    public CommentsService(TicketSupportSystemContext context)
    {
        _context = context;
    }

    public async Task<Guid> CreateComment(CreateCommentDTO commentDTO)
    {
        var newComment = Mappers.MapToComment(commentDTO);
        newComment.UpdateLastModified();

        _context.Comments.Add(newComment);
        await _context.SaveChangesAsync();

        return newComment.Id;
    }

    public async Task DeleteComment(Guid id)
    {
        var comment = await _context.Comments.FindAsync(id);
        if (comment == null)
        {
            throw new NotFoundException();
        }
        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
    }

    public async Task<CommentDTO> GetComment(Guid id)
    {
        var comments = await _context.Comments
            .Include(comment => comment.User)
            .Include(comment => comment.Ticket)
            .ToListAsync();

        var comment = comments.FirstOrDefault(t => t.Id == id);
        if (comment == null)
        {
            throw new NotFoundException();
        }

        var commentDto = Mappers.MapToCommentDTO(comment);

        return commentDto;
    }

    public async Task<IEnumerable<CommentDTO>> GetCommentsToTicket(Guid ticketId)
    {
        var ticket = await _context.Tickets
        .Include(t => t.Comments)
            .ThenInclude(c => c.User)
        .SingleOrDefaultAsync(t => t.Id == ticketId);

        if (ticket == null)
        {
            throw new NotFoundException();
        }

        var comments = ticket.Comments;

        var commentsDTOs = comments.Select(Mappers.MapToCommentDTO).ToList();

        return commentsDTOs;
    }

    public async Task UpdateComment(Guid id, UpdateCommentDTO commentDTO)
    {
        var comment = await _context.Comments.FindAsync(id);
        if (comment == null)
        {
            throw new NotFoundException();
        }
        comment.UpdateText(commentDTO.Text);

        await _context.SaveChangesAsync();
    }
}