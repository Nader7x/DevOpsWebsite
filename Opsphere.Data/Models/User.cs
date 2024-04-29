using System.ComponentModel.DataAnnotations;

namespace Opsphere.Data.Models;

public enum UserRoles
{
    TeamLeader,
    Developer,
    Admin
}

public class User
{
    [Key]
    public int Id { get; init; }
    [Required]
    [StringLength(50)]
    public string? Username { get; init; }
    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string? Email { get; init; }
    [Required]
    public UserRoles Role { get; init; }
    [Required]
    [StringLength(100)]
    public string? Password { get; init; }

    public ICollection<ProjectDeveloper>? ProjectDevelopers { get; set; }
    // public ICollection<Notification>? UserNotifications { get; set; }
    
}