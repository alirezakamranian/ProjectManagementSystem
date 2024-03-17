using Domain.Constants.Notification;
using Domain.Entities.Common;
using Domain.Models.Dtos.OrganizationInvitation.Request;
using Domain.Models.ServiceResponses.OrganizationInvitation;
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
    public class OrganizationInvitationService(DataContext context) : IOrganizationInvitationService
    {

        private readonly DataContext _context = context;

        public async Task<InviteEmployeeServiceResponse> InviteEmployee(InviteEmployeeRequest request, string issuerEmail)
        {
            try
            {
                var targetUser = await _context.Users.Include(u => u.Notifications)
                                .FirstOrDefaultAsync(u => u.Email == request.UserEmail.ToLower());

                if (targetUser == null)
                    return new InviteEmployeeServiceResponse(
                         InviteEmployeeServiceResponseStatus.UserNotExists);

                targetUser.Notifications.Add(new Notification
                {
                    Type = NotificationTypes.Invite,
                    Title = "OrganizationInvitation",
                    Issuer = issuerEmail,
                    Description = request.Message
                });

                await _context.SaveChangesAsync();

                return new InviteEmployeeServiceResponse(
                     InviteEmployeeServiceResponseStatus.Success);
            }
            catch
            {
                return new InviteEmployeeServiceResponse(
                     InviteEmployeeServiceResponseStatus.InternalError);
            }
        }

        public async Task<AcceptOrganizationInvitationServiceResponse> AcceptOrganizationInvitation(AcceptInvitationRequest request, string email)
        {
            try
            {
                var user = await _context.Users.AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Email == email);

                var notification = await _context.Notifications
                    .Where(n => n.UserId == user.Id)
                    .FirstOrDefaultAsync(n => n.Id == int.Parse(request.InviteId));

                if (notification == null)
                    return new AcceptOrganizationInvitationServiceResponse(
                         AcceptOrganizationInvitationServiceResponseStatus.NotificationNotExists);

                _context.Notifications.Remove(notification);

                var issuer = await _context.Users.Include(u=>u.Notifications)
                    .FirstOrDefaultAsync(u => u.Email == notification.Issuer);

                issuer.Notifications.Add(new Notification
                {
                    Type=NotificationTypes.Notice,
                    Title="InvitationAccepted!",
                    Description=$"YourInvitationToUser [{email}] accepted!",
                    Issuer=email
                });

                await _context.SaveChangesAsync();

                return new AcceptOrganizationInvitationServiceResponse(
                     AcceptOrganizationInvitationServiceResponseStatus.Success);
            }
            catch
            {
                return new AcceptOrganizationInvitationServiceResponse(
                     AcceptOrganizationInvitationServiceResponseStatus.InternalError);
            }
        }

        public async Task<RejectOrganizationInvitationServiceResponse> RejectOrganizationInvitation(RejectInvitationRequest request, string email)
        {
            try
            {
                var user = await _context.Users.AsNoTracking()
                 .FirstOrDefaultAsync(u => u.Email == email);

                var notification = await _context.Notifications
                    .Where(n => n.UserId == user.Id)
                    .FirstOrDefaultAsync(n => n.Id == int.Parse(request.InviteId));

                if (notification == null)
                    return new RejectOrganizationInvitationServiceResponse(
                         RejectOrganizationInvitationServiceResponseStatus.NotificationNotExists);

                _context.Notifications.Remove(notification);

                var issuer = await _context.Users.Include(u => u.Notifications)
                  .FirstOrDefaultAsync(u => u.Email == notification.Issuer);

                issuer.Notifications.Add(new Notification
                {
                    Type = NotificationTypes.Notice,
                    Title = "InvitationRejected!",
                    Description = $"YourInvitationToUser [{email}] Rejected!",
                    Issuer = email
                });

                await _context.SaveChangesAsync();

                return new RejectOrganizationInvitationServiceResponse(
                     RejectOrganizationInvitationServiceResponseStatus.Success);
            }
            catch (Exception)
            {
                return new RejectOrganizationInvitationServiceResponse(
                     RejectOrganizationInvitationServiceResponseStatus.InternalError);
            }
        }
    }
}
