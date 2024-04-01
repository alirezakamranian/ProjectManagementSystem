using Domain.Models.ServiceResponses.UserNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.ApiServices
{
    public interface IUserNotificationService
    {
        public Task<GetNotificationsServiceResponse> GetUserNotifications(string userId);
        public Task<DeleteNotificationServiceResponse>DeleteAllUserNotifications(string userId);
    }
}
