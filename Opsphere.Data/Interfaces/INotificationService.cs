namespace Opsphere.Data.Interfaces;

public interface INotificationService
{
    Task SendNotification(int id, string messege);
}