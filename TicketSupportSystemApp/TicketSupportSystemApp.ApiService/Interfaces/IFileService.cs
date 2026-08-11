using TicketSupportSystemApp.ApiService.DTOs.Responses;
namespace TicketSupportSystemApp.ApiService.Interfaces;

public interface IFileService
{
    public Task<AttachmentDTO> SaveAttachment(IFormFile file, Guid commentId);
    public Task<List<AttachmentDTO>> GetAttachmentsToComment(Guid commentId);
    public Task DeleteAttachment(Guid id);
}
