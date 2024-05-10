using System.ComponentModel.DataAnnotations;

namespace Opsphere.Dtos.ReplyDto;

public class AddReplyDto
{
    [Required]
    public string? ReplyContent { get; set; }
}