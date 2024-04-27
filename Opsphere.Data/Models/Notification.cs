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
    public int id { get; set; }
    [Required]
    public  NotificationType Type { get; set; }
    [Required]
    [MaxLength(250)]
    public String Content { get; set; }
    public int userId { get; set; }
    private bool isRead { get; set; } = false;
}