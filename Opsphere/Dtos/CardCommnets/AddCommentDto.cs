using System.ComponentModel.DataAnnotations;

namespace Opsphere.Dtos.CardCommnets;

public class AddCommentDto
{
    [Required]
    public string? CommentContent { get; set; }
}