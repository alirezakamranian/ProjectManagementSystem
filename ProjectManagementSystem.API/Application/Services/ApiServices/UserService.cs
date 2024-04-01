using Domain.Models.Dtos.User.Response;
using Domain.Models.ServiceResponses.User;
using Domain.Services.ApiServices;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.ApiServices
{
    public class UserService(DataContext context) : IUserService
    {
        private readonly DataContext _context = context;

        public async Task<GetUserDetailsServiceResponse> GetUserDetails(string userId)
        {
            try
            {
                var notifications = await _context.Notifications
                    .AsNoTracking().Where(n => n.UserId == userId).ToListAsync();

                var resultNotifications = new List<NotificationForResponseDto>();
                foreach (var notif in notifications)
                {
                    resultNotifications.Add(new NotificationForResponseDto
                    {
                        Id = notif.Id.ToString(),
                        Type = notif.Type,
                        Title = notif.Title,
                        Description = notif.Description,
                        Issuer = notif.Issuer
                    });
                }

                return new GetUserDetailsServiceResponse(
                     GetUserDetailsServiceResponseStatus.Success)
                {
                    Notifications = resultNotifications,
                    User = await _context.Users.AsNoTracking()
                    .FirstOrDefaultAsync(u=>u.Id.Equals(userId))
                };

            }
            catch
            {
                return new GetUserDetailsServiceResponse(
                     GetUserDetailsServiceResponseStatus.InternalError);
            }
        }

    }
}


