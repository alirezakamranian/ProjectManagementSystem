using Domain.Models.Dtos.Notification;
using Domain.Services.InternalServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR;
using Infrastructure.Hubs;
namespace Application.Services.InternalServices
{
    public class RealTimeNotificationService(IHubContext<NotifHub> hub) : IRealTimeNotificationService
    {
        private readonly IHubContext<NotifHub> _hub = hub;

        public async Task SendNotification(NotificationForResponseDto notification, string userId)
        {
            await _hub.Clients.User(userId)
                .SendAsync("ReciveNotificationUpdate", notification);
        }
    }
}
