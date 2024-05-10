using Opsphere.Data.Models;

namespace Opsphere.Dtos.CardCommnets;

public class CardCommentDto
{
    public string? UserName { get; set; }
    public string CommentContent { get; set; }
    
    public ICollection<Reply>? Replies { get; set; }
}
