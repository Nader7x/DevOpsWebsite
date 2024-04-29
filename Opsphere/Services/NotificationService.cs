using Microsoft.AspNetCore.SignalR;
using Opsphere.Data.Interfaces;

namespace Opsphere.Services;

public class NotificationService  : Hub<INotificationService>
{

}