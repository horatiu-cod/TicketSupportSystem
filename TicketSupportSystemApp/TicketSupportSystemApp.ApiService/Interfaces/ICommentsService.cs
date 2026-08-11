using TicketSupportSystemApp.ApiService.DTOs.Requests;
using TicketSupportSystemApp.ApiService.DTOs.Responses;

namespace TicketSupportSystemApp.ApiService.Interfaces;
    public interface ICommentsService
    {
        public Task<Guid> CreateComment(CreateCommentDTO commentDTO);
        public Task UpdateComment(Guid id, UpdateCommentDTO commentDTO);
        public Task DeleteComment(Guid id);
        public Task<IEnumerable<CommentDTO>> GetCommentsToTicket(Guid ticketId);
        public Task<CommentDTO> GetComment(Guid id);
    }
