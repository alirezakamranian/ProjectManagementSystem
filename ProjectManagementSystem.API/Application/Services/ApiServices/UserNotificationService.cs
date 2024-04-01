using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Constants.Notification;
using Domain.Models.ServiceResponses.UserNotification;
using Domain.Services.ApiServices;
using Domain.Services.InternalServices;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
namespace Application.Services.ApiServices
{
    public class UserNotificationService(DataContext context) : IUserNotificationService
    {

        private readonly DataContext _context = context;

        public async Task<GetNotificationsServiceResponse> GetUserNotifications(string userId)
        {
            try
            {
                var notifications = await _context.Notifications
                    .AsNoTracking().Where(n => n.UserId.Equals(userId)).ToListAsync();

                return new GetNotificationsServiceResponse(
                    GetNotificationsServiceResponseStatus.Success)
                {
                    Notifications = notifications
                };
            }
            catch
            {
                return new GetNotificationsServiceResponse(
                     GetNotificationsServiceResponseStatus.InternalError);

            }
        }

        public async Task<DeleteNotificationServiceResponse> DeleteAllUserNotifications(string userId)
        {
            try
            {
                var ns = await _context.Notifications
                     .Where(n => n.UserId.Equals(userId) &&
                        n.Type.Equals(NotificationTypes.Notice)).ToListAsync();
                _context.Notifications
                  .RemoveRange(ns);

                await _context.SaveChangesAsync();

                return new DeleteNotificationServiceResponse(
                     DeleteNotificationServiceResponseStatus.Success);
            }
            catch
            {
                return new DeleteNotificationServiceResponse(
                     DeleteNotificationServiceResponseStatus.InternalError);
            }
        }
    }
}
