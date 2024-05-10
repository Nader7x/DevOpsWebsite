using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Opsphere.Data.Models;

public class Reply
{
    [Key]
    public int Id { get; set; } 
    
    [ForeignKey(nameof(User))]
    public int UserId { get; set; }
    [Required]
    [ForeignKey(nameof(CardComment))]
    public int CardCommentId { get; set; }
    [Required]                  
    [StringLength(500)]             
    public string? ReplyContent { get; set; }
    public CardComment? CardComment { get; set; }
    public User? User { get; set; }
}