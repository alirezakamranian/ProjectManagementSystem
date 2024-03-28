using Application.Services.InternalServices;
using Domain.Constants.Notification;
using Domain.Entities.Common;
using Domain.Models.Dtos.OrganizationInvitation.Request;
using Domain.Models.ServiceResponses.OrganizationInvitation;
using Domain.Services.ApiServices;
using Domain.Services.InternalServices;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.ApiServices
{
    public class OrganizationInvitationService(IInvitationPendingManager pendingManager, DataContext context) : IOrganizationInvitationService
    {
        private readonly IInvitationPendingManager _pendingManager = pendingManager;
        private readonly DataContext _context = context;


        public async Task<InviteEmployeeServiceResponse> InviteEmployee(InviteEmployeeRequest request, string issuerEmail)
        {
            try
            {
                var targetUser = await _context.Users.Include(u => u.Notifications)
                    .FirstOrDefaultAsync(u => u.Email.Equals(request.UserEmail.ToLower()));

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

                var notification = await _context.Notifications.AsNoTracking()
                    .FirstOrDefaultAsync(n => n.UserId == targetUser.Id);

                _context.InvitationPendings.Add(new InvitationPending
                {
                    NotificationId = notification.Id,
                    OrganizationId = Guid.Parse(request.OrganizationId)
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
                        .FirstOrDefaultAsync(n => n.Id.ToString() == request.InviteId);

                if (notification == null)
                    return new AcceptOrganizationInvitationServiceResponse(
                         AcceptOrganizationInvitationServiceResponseStatus.NotificationNotExists);

                var serviceResault = await _pendingManager.AcceptPending(notification.Id.ToString(), user.Id);

                if (serviceResault == "error")
                    return new AcceptOrganizationInvitationServiceResponse(
                         AcceptOrganizationInvitationServiceResponseStatus.InternalError);

                _context.Notifications.Remove(notification);

                var issuer = await _context.Users.Include(u => u.Notifications)
                    .FirstOrDefaultAsync(u => u.Email == notification.Issuer);

                issuer.Notifications.Add(new Notification
                {
                    Type = NotificationTypes.Notice,
                    Title = "InvitationAccepted!",
                    Description = $"YourInvitationToUser [{email}] accepted!",
                    Issuer = email
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
                        .FirstOrDefaultAsync(n => n.Id.ToString() == request.InviteId);

                if (notification == null)
                    return new RejectOrganizationInvitationServiceResponse(
                         RejectOrganizationInvitationServiceResponseStatus.NotificationNotExists);

                var serviceResault = await _pendingManager
                    .RejectPending(request.InviteId, user.Id);

                if (serviceResault == "error")
                    return new RejectOrganizationInvitationServiceResponse(
                     RejectOrganizationInvitationServiceResponseStatus.InternalError);

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
