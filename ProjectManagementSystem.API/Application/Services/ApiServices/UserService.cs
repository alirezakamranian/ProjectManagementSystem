using Domain.Models.ApiModels.User.Request;
using Domain.Models.ApiModels.User.Response;
using Domain.Models.ApiModels.UserNotification.Response;
using Domain.Models.ServiceResponses.Storage;
using Domain.Models.ServiceResponses.User;
using Domain.Services.ApiServices;
using Domain.Services.InternalServices;
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
        ILogger<UserService> logger,
        IStorageService storageService) : IUserService
    {
        private readonly DataContext _context = context;
        private readonly ILogger<UserService> _logger = logger;
        private readonly IStorageService _storageService = storageService;

        /// <summary>
        /// Used for gettig userDetails with Notifs count
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<GetUserDetailsServiceResponse> GetUserDetails(string userId)
        {
            try
            {
                var user = await _context.Users.AsNoTracking()
                     .FirstOrDefaultAsync(u => u.Id.Equals(userId));

                var notificationsCount = await _context.Notifications
                    .AsNoTracking().Where(n => n.UserId.Equals(userId)).CountAsync();

                var getUrlResponse = await _storageService.GetUrl(new()
                {
                    FileKey = user.Id
                });

                return new GetUserDetailsServiceResponse(
                     GetUserDetailsServiceResponseStatus.Success)
                {
                    NotificationsCount = notificationsCount,
                    User = user,
                    AvatarUrl = getUrlResponse.Url
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("GetUserDetails : {Message}", ex.Message);

                return new GetUserDetailsServiceResponse(
                     GetUserDetailsServiceResponseStatus.InternalError);
            }
        }

        public async Task<UpdateUserProfileServiceResponse> UpdateUserProfile(UpdateUserProfileRequest request, string userId)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Id
                        .Equals(request.UserId));

                if (user == null)
                    return new UpdateUserProfileServiceResponse(
                         UpdateUserProfileServiceResponseStatus.UserNotExists);

                if (user.Id != userId)
                    return new UpdateUserProfileServiceResponse(
                         UpdateUserProfileServiceResponseStatus.AccessDenied);

                user.FullName = request.FullName;

                await _context.SaveChangesAsync();

                return new UpdateUserProfileServiceResponse(
                     UpdateUserProfileServiceResponseStatus.Success);
            }
            catch (Exception ex)
            {
                _logger.LogError("UpdateUserProfileService : {Message}", ex.Message);

                return new UpdateUserProfileServiceResponse(
                   UpdateUserProfileServiceResponseStatus.InternalError);
            }
        }
    }
}


