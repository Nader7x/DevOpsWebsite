using Opsphere.Data.Models;

namespace Opsphere.Data.Interfaces;

public interface INotificationRepository : IBaseRepository<Notification>
{
    Task<List<Notification>> UserNotificationsById(int userId);

}