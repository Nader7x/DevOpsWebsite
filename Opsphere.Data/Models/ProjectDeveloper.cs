using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Opsphere.Data.Models;

public class ProjectDeveloper
{
    [Key]
    [Column(Order = 0)]
    [ForeignKey(nameof(Project))]
    public int ProjectId { get; set; }
    [Key]
    [Column(Order = 1)]
    [ForeignKey(nameof(User))]
    public int UserId { get; set; }

    public bool IsTeamLeader { get; set; } = false;
   
    public User? User { get; set; }
    public ICollection<Card>? AssignedCards { get; set; }
}