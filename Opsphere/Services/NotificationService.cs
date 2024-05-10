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
        userConnections.Add(Context.ConnectionId,Context.User.GetNameId());
        Console.WriteLine("Key Count"+userConnections.Keys.Count);
        var id = Context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
        Console.WriteLine(id);
        foreach (var keyval in userConnections)
        {
            if (keyval.Value == id)
            {
                Clients.Client(keyval.Key).SendNotification(new Notification()
                {
                    UserId = 1,
                    Content = $"Welcome Again bro {Context.User.GetUsername()}"
                }); 
            }
    
        }

        // Console.WriteLine(Context.User.GetNameId());
        // Console.WriteLine(Context.User.GetUsername());
        // Console.WriteLine(Context.User.GetEmail());
        // Console.WriteLine(Context.User.GetRole());
        return base.OnConnectedAsync();
    }
}