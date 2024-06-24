﻿using Application.Services.InternalServices;
using Domain.Constants.AuthorizationResponses;
using Domain.Constants.Notification;
using Domain.Constants.Roles.OrganiationEmployees;
using Domain.Entities.Common;
using Domain.Models.ApiModels.OrganizationInvitation.Request;
using Domain.Models.ServiceResponses.Organization;
using Domain.Models.ServiceResponses.OrganizationInvitation;
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
    public class OrganizationInvitationService(DataContext context,
        IInvitationPendingManager pendingManager,
            ILogger<OrganizationInvitationService> logger,
                IAuthorizationService authService) : IOrganizationInvitationService
    {
        private readonly DataContext _context = context;
        private readonly ILogger<OrganizationInvitationService> _logger = logger;
        private readonly IInvitationPendingManager _pendingManager = pendingManager;
        private readonly IAuthorizationService _authService = authService;

        /// <summary> 
        /// Searchs users across the project
        /// </summary>
        /// <param name="requst"></param>
        /// <returns>SearchUserServiceResponse</returns>
        public async Task<SearchUserServiceResponse> SearchUser(SearchUserRequst requst)
        {
            try
            {
                return new SearchUserServiceResponse(
                    SearchUserServiceResponseStatus.Success)
                {
                    Users = await _context.Users
                        .AsNoTracking().Where(u => u.Email
                            .Contains(requst.Query.ToLower().Trim()) ||
                                 u.FullName.Contains(requst.Query
                                    .ToLower().Trim())).ToListAsync()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("SearchUserService {Message}", ex.Message);

                return new SearchUserServiceResponse(
                     SearchUserServiceResponseStatus.InternalError);
            }
        }

        /// <summary>
        /// Invites user to organization
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns>InviteEmployeeServiceResponse</returns>
        public async Task<InviteEmployeeServiceResponse> InviteEmployee(InviteEmployeeRequest request, string userId)
        {
            try
            {
                var authResult = await _authService
                    .AuthorizeByOrganizationId(Guid.Parse(request.OrganizationId),
                        userId, [OrganizationEmployeesRoles.Member]);

                if (authResult.Equals(AuthorizationResponse.Deny))
                    return new InviteEmployeeServiceResponse(
                         InviteEmployeeServiceResponseStatus.AccessDenied);

                var issuerUser = await _context.Users
                    .Include(u => u.Notifications)
                        .FirstOrDefaultAsync(u => u.Id.Equals(userId));

                var targetUser = await _context.Users
                    .Include(u => u.Notifications)
                        .FirstOrDefaultAsync(u => u.Email
                            .Equals(request.UserEmail.ToLower()));

                if (targetUser == null)
                    return new InviteEmployeeServiceResponse(
                         InviteEmployeeServiceResponseStatus.UserNotExists);

                if (await _context.OrganizationEmployees
                    .AnyAsync(e => e.OrganizationId.ToString()
                        .Equals(request.OrganizationId) && 
                            e.UserId.Equals(targetUser.Id)))
                    return new InviteEmployeeServiceResponse(
                         InviteEmployeeServiceResponseStatus.UserIsAlredyEmployeeOfThisOrganization);

                var existInvites = targetUser.Notifications
                    .Where(n => n.Issuer.Equals(issuerUser.Email)).ToList();

                if (existInvites != null)
                {
                    foreach (var n in existInvites)
                    {
                        if (await _context.InvitationPendings
                            .AnyAsync(p => p.NotificationId.Equals(
                                n.Id) && p.OrganizationId.ToString()
                                    .Equals(request.OrganizationId)))
                            return new InviteEmployeeServiceResponse(
                                 InviteEmployeeServiceResponseStatus.UserAlredyInvited);
                    }
                }

                targetUser.Notifications.Add(new Notification
                {
                    Type = NotificationTypes.Invite,
                    Title = "OrganizationInvitation",
                    Issuer = issuerUser.Email,
                    Description = request.Message
                });

                await _context.SaveChangesAsync();

                var notification = await _context.Notifications
                    .AsNoTracking().FirstOrDefaultAsync(n =>
                        n.UserId.Equals(targetUser.Id) &&
                            n.Issuer.Equals(issuerUser.Email)&&
                                n.Type.Equals(NotificationTypes.Invite));

                _context.InvitationPendings.Add(new InvitationPending
                {
                    NotificationId = notification.Id,
                    OrganizationId = Guid.Parse(request.OrganizationId)
                });

                await _context.SaveChangesAsync();

                return new InviteEmployeeServiceResponse(
                     InviteEmployeeServiceResponseStatus.Success);
            }
            catch (Exception ex)
            {
                _logger.LogError("InviteEmployeeService {Message}", ex.Message);

                return new InviteEmployeeServiceResponse(
                     InviteEmployeeServiceResponseStatus.InternalError);
            }
        }

        /// <summary>
        /// Accepts income invitation
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns>AcceptOrganizationInvitationServiceResponse</returns>
        public async Task<AcceptOrganizationInvitationServiceResponse> AcceptOrganizationInvitation(AcceptInvitationRequest request, string userId)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.Notifications)
                        .FirstOrDefaultAsync(u => u.Id.Equals(userId));

                var notification = await _context.Notifications
                    .Where(n => n.UserId.Equals(userId))
                        .FirstOrDefaultAsync(n =>
                            n.Id.ToString() == request.InviteId);

                if (notification == null)
                    return new AcceptOrganizationInvitationServiceResponse(
                         AcceptOrganizationInvitationServiceResponseStatus.NotificationNotExists);

                var serviceResault = await _pendingManager
                    .AcceptPending(notification.Id.ToString(), userId);

                if (serviceResault == "error")
                    return new AcceptOrganizationInvitationServiceResponse(
                         AcceptOrganizationInvitationServiceResponseStatus.InternalError);

                _context.Notifications.Remove(notification);

                var issuer = await _context.Users
                    .Include(u => u.Notifications)
                        .FirstOrDefaultAsync(u =>
                            u.Email.Equals(notification.Issuer));

                issuer.Notifications.Add(new Notification
                {
                    Type = NotificationTypes.Notice,
                    Title = "InvitationAccepted!",
                    Description = $"YourInvitationToUser [{user.Email}] accepted!",
                    Issuer = user.Email
                });

                await _context.SaveChangesAsync();

                return new AcceptOrganizationInvitationServiceResponse(
                     AcceptOrganizationInvitationServiceResponseStatus.Success);
            }
            catch (Exception ex)
            {
                _logger.LogError("AccpetInvitationService : {Message}", ex.Message);

                return new AcceptOrganizationInvitationServiceResponse(
                     AcceptOrganizationInvitationServiceResponseStatus.InternalError);
            }
        }

        /// <summary>
        /// Rejects income invitation
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns>RejectOrganizationInvitationServiceResponse</returns>
        public async Task<RejectOrganizationInvitationServiceResponse> RejectOrganizationInvitation(RejectInvitationRequest request, string userId)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.Notifications)
                        .FirstOrDefaultAsync(u => u.Id.Equals(userId));

                var notification = await _context.Notifications
                    .Where(n => n.UserId.Equals(user.Id))
                        .FirstOrDefaultAsync(n => n.Id.ToString()
                            .Equals(request.InviteId));

                if (notification == null)
                    return new RejectOrganizationInvitationServiceResponse(
                         RejectOrganizationInvitationServiceResponseStatus.NotificationNotExists);

                var serviceResault = await _pendingManager
                    .RejectPending(request.InviteId, userId);

                if (serviceResault.Equals("error"))
                    return new RejectOrganizationInvitationServiceResponse(
                         RejectOrganizationInvitationServiceResponseStatus.InternalError);

                _context.Notifications.Remove(notification);

                var issuer = await _context.Users
                    .Include(u => u.Notifications)
                        .FirstOrDefaultAsync(u => u.Email
                            .Equals(notification.Issuer));

                issuer.Notifications.Add(new Notification
                {
                    Type = NotificationTypes.Notice,
                    Title = "InvitationRejected!",
                    Description = $"YourInvitationToUser [{user.Email}] Rejected!",
                    Issuer = user.Email
                });

                await _context.SaveChangesAsync();

                return new RejectOrganizationInvitationServiceResponse(
                     RejectOrganizationInvitationServiceResponseStatus.Success);
            }
            catch (Exception ex)
            {
                _logger.LogError("RejectInvitationService : {Message}", ex.Message);

                return new RejectOrganizationInvitationServiceResponse(
                     RejectOrganizationInvitationServiceResponseStatus.InternalError);
            }
        }
    }
}
