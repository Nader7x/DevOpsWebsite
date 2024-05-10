using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Opsphere.Data.Interfaces;
using Opsphere.Data.Models;
using Opsphere.Helpers;

namespace Opsphere.Services;

// [Authorize]
public class NotificationService : Hub<INotificationService>
{
 
    public override Task OnConnectedAsync()
    {
            Clients.User(Context.User.GetNameId()).SendNotification(new Notification()
            {
                UserId = 1,
                Content = $"Kosom Elbdan {Context.User.GetNameId()}"
            });
        return base.OnConnectedAsync();
    }
}