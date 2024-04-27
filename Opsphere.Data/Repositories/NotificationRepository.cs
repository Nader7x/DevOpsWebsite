using Opsphere.Data.Models;
using Opsphere.Interfaces;

namespace Opsphere.Data.Repositories;

public class NotificationRepository(ApplicationDbContext dbContext) : BaseRepository<Notification>(dbContext), INotificationRepository
{
    
}