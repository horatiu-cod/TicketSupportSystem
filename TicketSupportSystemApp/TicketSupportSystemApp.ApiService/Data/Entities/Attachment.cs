namespace TicketSupportSystemApp.ApiService.Data.Entities;
public sealed class Attachment : EntityBase
{

    public string FileName { get; private set; }
    public string Path { get; private set; } 
    public Guid CommentId { get; private set; }
    public Comment Comment { get; private set; } = null!;

// For EF Core
    private Attachment()
    {
        FileName = string.Empty;
        Path = string.Empty;
    } 
    private Attachment(string fileName, string path, Guid commentId)
    {
        FileName = fileName;
        Path = path;
        CommentId = commentId;
    }

    public static Attachment Create(string fileName, string path, Guid commentId)
    {
        ValidateInputs(fileName, path, commentId);
        return new Attachment(fileName, path, commentId);
    }
    public void Update(string newFileName)
    {
        if (string.IsNullOrWhiteSpace(newFileName))
        {
            throw new ArgumentException("File name cannot be null or empty.", nameof(newFileName));
        }
        FileName = newFileName;
        UpdateLastModified();
    }

     private static void ValidateInputs(string fileName, string path, Guid commentId)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException("File name cannot be null or empty.", nameof(fileName));
        }

        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException("Path cannot be null or empty.", nameof(path));
        }

        if (commentId == Guid.Empty)
        {
            throw new ArgumentException("Comment ID cannot be empty.", nameof(commentId));
        }
    }
}
