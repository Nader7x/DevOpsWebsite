using System.ComponentModel.DataAnnotations;

namespace Opsphere.Dtos.Attachment;

public class CreateAttachmentDto
{
    [Required][StringLength(100)] public string? FileName { get; set; }
    [Required] public byte[]? File { get; set; }
}