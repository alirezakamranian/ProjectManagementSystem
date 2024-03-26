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

        public async Task<GetUserDetailsServiceResponse> GetUserDetails(string email)
        {
            try
            {
                var user = await _context.Users.Include(u => u.Notifications).FirstOrDefaultAsync(u => u.Email == email);
                var notifications = _context.Notifications.Where(n => n.UserId == user.Id).ToList();

                var resultNotifications = new List<NotificationForResponseDto>();
                foreach (var notif in notifications)
                {
                    resultNotifications.Add(new NotificationForResponseDto
                    {
                        Id = notif.Id,
                        Type = notif.Type,
                        Title = notif.Title,
                        Description = notif.Description,
                        Issuer=notif.Issuer
                    });
                }

                return new GetUserDetailsServiceResponse(
                     GetUserDetailsServiceResponseStatus.Success)
                {
                    Notifications = resultNotifications
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


