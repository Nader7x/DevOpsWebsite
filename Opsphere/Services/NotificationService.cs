using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Opsphere.Data.Interfaces;
using Opsphere.Data.Models;
using Opsphere.Helpers;

namespace Opsphere.Services;

public class NotificationService(IUnitOfWork unitOfWork)  : Hub<INotificationService>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public override async Task OnConnectedAsync()
    {
        await Clients.Client(Context.ConnectionId).SendNotification("Kosom el net bta3y we el frontend");
    }

    // public async Task SendNotificationToUser(string userId, string notification)
    // {
    //     // var user = await _unitOfWork.UserRepository.GetByIdAsync(int.Parse(userId));
    //     // if (Context.User != null)
    //     //     if (Context.User.GetUsername() == user?.Username)
    //     //     {
    //             // await Clients.User(userId).SendNotification(userId, notification);
    //             await Clients.All.SendNotification(userId, notification);
    //         // }
    // }

}