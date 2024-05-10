using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Opsphere.Data.Models;

public class CardComment
{
    [Key]
    public int Id { get; set; } 
    [ForeignKey(nameof(User))]
    public int UserId { get; set; }
    [Required]
    [ForeignKey(nameof(Card))]
    public int CardId { get; set; }
    [Required]                  
    [StringLength(500)]             
    public string? CommentContent { get; set; }
    
    public ICollection<Reply> Replies { get; set; }
    public Card? Card { get; set; }
    public User? User { get; set; }
}