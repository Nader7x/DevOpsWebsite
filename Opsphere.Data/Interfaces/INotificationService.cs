using Opsphere.Data.Models;

namespace Opsphere.Data.Interfaces;

public interface INotificationService
{
    Task SendNotification(Notification notification);
}