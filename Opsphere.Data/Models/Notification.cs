using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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
    [JsonConverter(typeof(StringEnumConverter))]
    public  NotificationType Type { get; set; }
    [Required]
    [MaxLength(250)]
    public string? Content { get; set; }
    public int UserId { get; set; }

    public DateTime NotificationDate { get; set; } = DateTime.UtcNow;
    public bool IsRead { get; set; } = false;
}