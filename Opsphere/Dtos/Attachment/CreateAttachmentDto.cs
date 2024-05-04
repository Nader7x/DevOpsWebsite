using System.ComponentModel.DataAnnotations;

namespace Opsphere.Dtos.Attachment;

public class CreateAttachmentDto
{
    [Required]public IFormFile? File { get; set; }
}