using Opsphere.Data.Models;
using Opsphere.Dtos.ReplyDto;

namespace Opsphere.Dtos.CardCommnets;

public class CardCommentDto
{
    public int Id { get; set; }
    public string? UserName { get; set; }
    public string? CommentContent { get; set; }
    
    public ICollection<GetReplyDto>? Replies { get; set; }
}
