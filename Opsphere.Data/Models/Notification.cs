using System.ComponentModel.DataAnnotations;

namespace Opsphere.Data.Models;

public enum NotificationType
{
    Info,
    Warning,
    Error,
    ProjectInvite,
    Other
}
public class Notification
{
    [Key]
    public int Id { get; set; }
    [Required]
    public  NotificationType Type { get; set; }
    [Required]
    [MaxLength(250)]
    public string? Content { get; set; }
    public int UserId { get; set; }

    public DateTime NotificationDate { get; set; } = DateTime.UtcNow;
    private bool IsRead { get; set; } = false;
}