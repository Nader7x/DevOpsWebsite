namespace Opsphere.Data.Interfaces;

public interface INotificationService
{
    Task SendNotificationToUser(string userId,string notification);
    Task SendNotification(string notification);
}