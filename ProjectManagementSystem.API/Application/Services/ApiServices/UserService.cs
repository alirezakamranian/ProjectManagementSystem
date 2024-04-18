using Domain.Models.Dtos.User.Response;
using Domain.Models.Dtos.UserNotification.Response;
using Domain.Models.ServiceResponses.User;
using Domain.Services.ApiServices;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.ApiServices
{
    public class UserService(DataContext context,
        ILogger<UserService> logger) : IUserService
    {
        private readonly DataContext _context = context;
        private readonly ILogger<UserService> _logger = logger;

        public async Task<GetUserDetailsServiceResponse> GetUserDetails(string userId)
        {
            try
            {
                var notificationsCount = await _context.Notifications
                    .AsNoTracking().Where(n => n.UserId.Equals(userId)).CountAsync();

                return new GetUserDetailsServiceResponse(
                     GetUserDetailsServiceResponseStatus.Success)
                {
                    NotificationsCount = notificationsCount,
                    User = await _context.Users.AsNoTracking()
                     .FirstOrDefaultAsync(u => u.Id.Equals(userId))
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("GetUserDetails : {Message}", ex.Message);

                return new GetUserDetailsServiceResponse(
                     GetUserDetailsServiceResponseStatus.InternalError);
            }
        }
    }
}


