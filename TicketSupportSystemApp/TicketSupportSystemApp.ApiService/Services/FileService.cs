using Microsoft.EntityFrameworkCore;
using TicketSupportSystemApp.ApiService.Data;
using TicketSupportSystemApp.ApiService.Data.Entities;
using TicketSupportSystemApp.ApiService.DTOs.Responses;
using TicketSupportSystemApp.ApiService.Interfaces;
using TicketSupportSystemApp.ApiService.Common.Exceptions;

namespace TicketSupportSystemApp.ApiService.Services;
public class FileService(IWebHostEnvironment env, TicketSupportSystemContext context) : IFileService
{
    private readonly string _attachmentsDir = "attachments";
    private readonly TicketSupportSystemContext _context = context;
    private readonly IWebHostEnvironment _env = env;

    public async Task<AttachmentDTO> SaveAttachment(IFormFile file, Guid commentId)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File not selected");
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var fullPath = Path.Combine(_env.WebRootPath, _attachmentsDir, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var filePath = Path.Combine(_attachmentsDir, fileName);

            var attachment = Attachment.Create(fileName, filePath, commentId);

            _context.Attachments.Add(attachment);
            await _context.SaveChangesAsync();

            var attachmentDTO = Mappers.MapToAttachmentDTO(attachment);

            return attachmentDTO;
        }

    public async Task<List<AttachmentDTO>> GetAttachmentsToComment(Guid commentId)
    {
        var comment = await _context.Comments
            .Include(c => c.Attachments)
            .SingleOrDefaultAsync(c => c.Id == commentId);
        if (comment == null)
        {
            throw new NotFoundException();
        }

        var attachments = comment.Attachments.ToList();

        var attachmentsDTOs = attachments.Select(Mappers.MapToAttachmentDTO).ToList();

        return attachmentsDTOs;
    }

    public async Task DeleteAttachment(Guid id)
    {
        var attachment = await _context.Attachments
            .FirstOrDefaultAsync(f => f.Id == id);

        if (attachment == null)
        {
            throw new NotFoundException("File not found");
        }

        var fullPath = Path.Combine(_env.WebRootPath, attachment.Path);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }

        _context.Attachments.Remove(attachment);
        await _context.SaveChangesAsync();
    }
}
