using System.ComponentModel.DataAnnotations;

namespace Opsphere.Dtos.Attachment;

public class AttachmentDto
{
    public int CardId { get; set; }
    public string? FilePath { get; set; }
    public IFormFile? File { get; set; }
    public string? FileUrl { get; set; }
}