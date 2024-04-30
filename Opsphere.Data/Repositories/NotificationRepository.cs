using Microsoft.EntityFrameworkCore;
using Opsphere.Data.Interfaces;
using Opsphere.Data.Models;

namespace Opsphere.Data.Repositories;

public class NotificationRepository(ApplicationDbContext dbContext) : BaseRepository<Notification>(dbContext), INotificationRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<List<Notification>> UserNotificationsById(int userId)
    {
       return await _dbContext.Notifications.Where(n => n.userId == userId).ToListAsync();
    }
}