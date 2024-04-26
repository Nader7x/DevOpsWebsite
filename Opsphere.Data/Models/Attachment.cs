using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Opsphere.Data.Models;

public class Attachment
{
     [Key] public int Id { get; set; }
     [Required] public int CardId { get; set; }
     [Required][StringLength(100)] public string? FileName { get; set; }
     [Required] public byte[]? File { get; set; }
     [ForeignKey(nameof(CardId))] public Card? Card { get; set; }
}