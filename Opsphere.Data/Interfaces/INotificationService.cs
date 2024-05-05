namespace Opsphere.Data.Interfaces;

public interface INotificationService
{
    // Task SendNotification(string userId,string notification);
    Task SendNotification(string notification);
}