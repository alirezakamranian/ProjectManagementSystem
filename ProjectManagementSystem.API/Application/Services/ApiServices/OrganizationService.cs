﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.HumanResource;
using Domain.Entities.Common;
using Domain.Models.ApiModels.Organization.Response;
using Domain.Services.ApiServices;
using Infrastructure.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Domain.Constants.Notification;
using Domain.Models.ServiceResponses.Organization;
using Domain.Constants.Roles.OrganiationEmployees;
using Microsoft.Extensions.Logging;
using Domain.Models.ApiModels.Organization.Request;
using Domain.Models.Dtos.Project;
using Domain.Models.Dtos.Organization;
using Domain.Constants.AuthorizationResponses;
using Domain.Services.InternalServices;
namespace Application.Services.ApiServices
{
    public class OrganizationService(DataContext context,
        ILogger<AuthenticationService> logger,
            IAuthorizationService authService) : IOrganizationService
    {
        private readonly DataContext _context = context;
        private readonly ILogger<AuthenticationService> _logger = logger;
        private readonly IAuthorizationService _authService = authService;

        public async Task<CreateOrganizationServiceResponse> CreateOrganization(CreateOrganizationRequest request, string userId)
        {
            try
            {
                if (await _context.Organizations
                    .AnyAsync(o => o.OwnerId.Equals(userId) &&
                        o.Name.Equals(request.Name)))
                    return new CreateOrganizationServiceResponse(
                         CreateOrganizationServiceResponseStatus.OrganizationExists);

                var user = await _context.Users
                    .Include(u => u.Organizations)
                        .FirstOrDefaultAsync(u => u.Id
                            .Equals(userId));

                user.Organizations.Add(new Organization
                {
                    Name = request.Name
                });

                await _context.SaveChangesAsync();

                var org = await _context.Organizations
                    .Include(o => o.OrganizationEmployees)
                        .FirstOrDefaultAsync(o => o.OwnerId
                            .Equals(userId) && o.Name.Equals(request.Name));

                org.OrganizationEmployees.Add(new OrganizationEmployee
                {
                    UserId = userId,
                    Role = OrganizationEmployeesRoles.Leader
                });

                await _context.SaveChangesAsync();

                return new CreateOrganizationServiceResponse(
                     CreateOrganizationServiceResponseStatus.Success);
            }
            catch (Exception ex)
            {
                _logger.LogError("CreateOrganizationService : {Message}", ex.Message);

                return new CreateOrganizationServiceResponse(
                     CreateOrganizationServiceResponseStatus.InternalError);
            }

        }

        public async Task<UpdateOrganizationServiceResponse> UpdateOrganization(UpdateOrganizationRequest request, string userId)
        {
            try
            {

                var org = _context.Organizations
                    .Where(o => o.Id.ToString()
                        .Equals(request.OrganizationId) &&
                             o.OwnerId.Equals(userId)).FirstOrDefault();

                if (org == null)
                    return new UpdateOrganizationServiceResponse(
                         UpdateOrganizationServiceResponseStatus.OrganizationNotExists);

                var authResult = await _authService
                    .AuthorizeByOrganizationId(org.Id, userId,
                        [OrganizationEmployeesRoles.Member,
                            OrganizationEmployeesRoles.Admin]);

                if (authResult.Equals(AuthorizationResponse.Deny))
                    return new UpdateOrganizationServiceResponse(
                         UpdateOrganizationServiceResponseStatus.AccessDenied);

                    org.Name = request.NewName;

                await _context.SaveChangesAsync();

                return new UpdateOrganizationServiceResponse(
                     UpdateOrganizationServiceResponseStatus.Success);
            }
            catch (Exception ex)
            {
                _logger.LogError("UpdateOrganizationService : {Message}", ex.Message);

                return new UpdateOrganizationServiceResponse(
                     UpdateOrganizationServiceResponseStatus.InternalError);
            }
        }

        public async Task<GetOrganizationServiceResponse> GetOrganization(GetOrganizationRequest request)
        {
            try
            {
                var org = await _context.Organizations.Include(o => o.Projects)
                    .Include(o => o.OrganizationEmployees)
                        .Include(o => o.OrganizationEmployees)
                            .ThenInclude(e => e.User).AsNoTracking()
                                .FirstOrDefaultAsync(o => o.Id.ToString()
                                    .Equals(request.OrganizationId));

                if (org == null)
                    return new GetOrganizationServiceResponse(
                         GetOrganizationServiceResponseStatus.OrganizationNotExists);

                if (!org.OrganizationEmployees
                    .Any(e => e.UserId.Equals(request.UserId)))
                    return new GetOrganizationServiceResponse(
                         GetOrganizationServiceResponseStatus.AccessDenied);

                return new GetOrganizationServiceResponse(
                     GetOrganizationServiceResponseStatus.Success)
                {
                    Name = org.Name,
                    Projects = org.Projects,
                    Employees = org.OrganizationEmployees
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("GetOrganizationService : {Message}", ex.Message);

                return new GetOrganizationServiceResponse(
                     GetOrganizationServiceResponseStatus.InternalError);
            }
        }

        public async Task<GetSubscribedOrganizationsServiceResponse> GetSubscribedOrganizations(string userId)
        {
            try
            {
                var memberOf = await _context.OrganizationEmployees
                    .Where(e => e.UserId.Equals(userId)).ToListAsync();

                List<OrganizationForResponsteDto> userOrgs = [];

                foreach (var e in memberOf)
                {
                    var org = await _context.Organizations
                        .Include(o => o.Projects).AsNoTracking()
                            .Where(o => o.Id.Equals(e.OrganizationId))
                                .FirstOrDefaultAsync();

                    List<MinimumValueProjectDto> projects = [];

                    foreach (var p in org.Projects)
                    {
                        projects.Add(new()
                        {
                            Id = p.Id.ToString(),
                            Name = p.Name,
                        });
                    }

                    userOrgs.Add(new OrganizationForResponsteDto()
                    {
                        Id = org.Id.ToString(),
                        Name = org.Name,
                        Projects = projects,
                        LeaderId = org.OwnerId
                    });
                }

                var response = new GetSubscribedOrganizationsServiceResponse(
                    GetSubscribedOrganizationsServiceResponseStatus.Success)
                {
                    Organizations = userOrgs
                };

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("GetSubscribedOrganizationService {Message}", ex.Message);

                return new GetSubscribedOrganizationsServiceResponse(
                     GetSubscribedOrganizationsServiceResponseStatus.InternalError);
            }
        }

        public async Task<RemoveOrganizationServiceResponse> RemoveOrganization(RemoveOrganizationRequest request, string userId)
        {
            try
            {
                var org = _context.Organizations.Include(o =>
                    o.Projects).ThenInclude(p => p.ProjectMembers)
                        .Where(o => o.Id.ToString()
                            .Equals(request.OrganizationId)).FirstOrDefault();

                if (org == null)
                    return new RemoveOrganizationServiceResponse(
                         RemoveOrganizationServiceResponseStatus.OrganizationNotExists);

                var authResult = await _authService
                    .AuthorizeByOrganizationId(org.Id, userId,
                        [OrganizationEmployeesRoles.Member,
                         OrganizationEmployeesRoles.Admin]);

                if (authResult.Equals(AuthorizationResponse.Deny))
                    return new RemoveOrganizationServiceResponse(
                        RemoveOrganizationServiceResponseStatus.AccessDenied);

                foreach (var p in org.Projects)
                {
                    var members = await _context.ProjectMembers
                        .Where(pm => pm.ProjectId.Equals(p.Id)).ToListAsync();

                    _context.ProjectMembers.RemoveRange(members);
                }

                _context.Organizations.Remove(org);

                await _context.SaveChangesAsync();

                return new RemoveOrganizationServiceResponse(
                     RemoveOrganizationServiceResponseStatus.Success);
            }
            catch (Exception ex)
            {
                _logger.LogError("RemoveOrganizationService {Message}", ex.Message);

                return new RemoveOrganizationServiceResponse(
                     RemoveOrganizationServiceResponseStatus.InternalError);
            }
        }
    }
}
