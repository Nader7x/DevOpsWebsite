using Microsoft.EntityFrameworkCore;
using Opsphere.Data.Interfaces;
using Opsphere.Data.Models;

namespace Opsphere.Data.Repositories;

public class NotificationRepository(ApplicationDbContext dbContext) : BaseRepository<Notification>(dbContext), INotificationRepository
{
    public async Task<List<Notification>> UserNotificationsById(int userId)
    {
       return await dbContext.Notifications.Where(n => n.userId == userId).ToListAsync();
    }
}