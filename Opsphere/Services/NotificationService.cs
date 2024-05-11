using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Opsphere.Data.Interfaces;
using Opsphere.Data.Models;
using Opsphere.Helpers;


namespace Opsphere.Services;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class NotificationService : Hub<INotificationService>
{
    public Dictionary<string, string> userConnections = new();

    public override Task OnConnectedAsync()
    {
        if (Context.User.Identity.IsAuthenticated)
        {
            userConnections.Add(Context.ConnectionId,Context.User.GetNameId());
            // var id = Context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            // foreach (var keyval in userConnections)
            // {
            //     if (keyval.Value == id)
            //     {
            //         Clients.Client(keyval.Key).SendNotification(new Notification()
            //         {
            //             UserId = int.Parse(Context.User.GetNameId()),
            //             Content = $"Welcome Again bro {Context.User.GetUsername()}"
            //         }); 
            //     }
            // }
        }
        return base.OnConnectedAsync();
    }
}