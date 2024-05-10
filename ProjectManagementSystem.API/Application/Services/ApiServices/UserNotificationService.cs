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
using Microsoft.Extensions.Logging;
namespace Application.Services.ApiServices
{
    public class UserNotificationService(DataContext context,
         ILogger<UserNotificationService> logger) : IUserNotificationService
    {

        private readonly DataContext _context = context;
        private readonly ILogger<UserNotificationService> _logger = logger;

        /// <summary>
        /// Used for getting all user notifications
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
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
            catch (Exception ex)
            {
                _logger.LogError("GetUserNotificationsService : {Message}", ex.Message);

                return new GetNotificationsServiceResponse(
                     GetNotificationsServiceResponseStatus.InternalError);
            }
        }

        /// <summary>
        /// Removes all user notifications exept Invitations
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<DeleteNotificationServiceResponse> DeleteAllUserNotifications(string userId)
        {
            try
            {
                var notifications = await _context.Notifications
                     .Where(n => n.UserId.Equals(userId) &&
                        n.Type.Equals(NotificationTypes.Notice)).ToListAsync();

                _context.Notifications
                  .RemoveRange(notifications);

                await _context.SaveChangesAsync();

                return new DeleteNotificationServiceResponse(
                     DeleteNotificationServiceResponseStatus.Success);
            }
            catch (Exception ex)
            {
                _logger.LogError("DeleteAllUserNotifications {Message}", ex.Message);

                return new DeleteNotificationServiceResponse(
                     DeleteNotificationServiceResponseStatus.InternalError);
            }
        }
    }
}
