using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace Opsphere.Data.Models;

public class Project
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string? Name { get; set; }
    [StringLength(500)]
    public string? Description { get; set; }
    public ICollection<Card>? Cards { get; set; }
    public ICollection<ProjectDeveloper> ProjectDevelopers { get; set; }
}