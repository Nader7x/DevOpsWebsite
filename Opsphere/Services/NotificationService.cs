using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Opsphere.Data.Interfaces;
using Opsphere.Data.Models;
using Opsphere.Helpers;

namespace Opsphere.Services;

public class NotificationService(IUnitOfWork unitOfWork)  : Hub<INotificationService>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task SendNotificationToUser(string userId, string notification)
    {
        // Implement your logic to send notification to specific user
        await Clients.User(userId).SendNotification(notification);
    }

    public async Task SendNotificationToAll(string notification)
    {
        // Implement your logic to send notification to all connected clients
        await Clients.All.SendNotification(notification);
    }

    public override async Task OnConnectedAsync()
    {
        await Clients.Client(Context.ConnectionId).SendNotification("Welcome to Opsphere Platform");
        await base.OnConnectedAsync();
    }
}