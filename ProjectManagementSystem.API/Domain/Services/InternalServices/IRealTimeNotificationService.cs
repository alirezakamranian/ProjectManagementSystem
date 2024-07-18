using Domain.Entities.Common;
using Domain.Models.Dtos.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.InternalServices
{
    public interface IRealTimeNotificationService
    {
        public Task SendNotification(NotificationForResponseDto notification,string userId);
    }
}
