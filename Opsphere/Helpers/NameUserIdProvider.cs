using Microsoft.AspNetCore.SignalR;

namespace Opsphere.Helpers;

public class NameUserIdProvider  : IUserIdProvider
{
    public string GetUserId(HubConnectionContext connection)
    {
        return connection.User?.Identity?.Name;
    }
}