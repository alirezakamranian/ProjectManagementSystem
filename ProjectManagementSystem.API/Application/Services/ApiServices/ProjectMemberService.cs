﻿using Domain.Models.ServiceResponses.ProjectMember;
using Domain.Services.ApiServices;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.HumanResource;
using Domain.Constants.Roles.OrganiationEmployees;
using Microsoft.Extensions.Logging;
using Domain.Models.ApiModels.ProjectMember.Request;
using Domain.Services.InternalServices;
using Domain.Constants.AuthorizationResponses;
namespace Application.Services.ApiServices
{
    public class ProjectMemberService(DataContext context,
        ILogger<AuthenticationService> logger,
            IAuthorizationService authService) : IProjectMemberService
    {

        private readonly DataContext _context = context;
        private readonly ILogger<AuthenticationService> _logger = logger;
        private readonly IAuthorizationService _authService = authService;

        public async Task<AddMemberServiceResponse> AddMember(AddMemberRequest request, string userId)
        {
            try
            {
                var project = await _context.Projects
                    .Include(p => p.ProjectMembers)
                        .FirstOrDefaultAsync(p =>
                            p.Id.ToString().Equals(request.ProjectId));

                if (project == null)
                    return new AddMemberServiceResponse(
                         AddMemberServiceResponseStatus.ProjectNotExists);

                var organization = await _context.Organizations
                    .Include(o => o.OrganizationEmployees)
                        .FirstOrDefaultAsync(o => o.Id
                            .Equals(project.OrganizationId));

                var authResult = await _authService
                    .AuthorizeByProjectId(project.Id, userId,
                        [ProjectMemberRoles.Member]);

                if (authResult.Equals(AuthorizationResponse.Deny))
                    return new AddMemberServiceResponse(
                         AddMemberServiceResponseStatus.AccessDenied);

                var employee = organization.OrganizationEmployees
                    .FirstOrDefault(e => e.Id.ToString().Equals(request.EmployeeId));

                if (employee == null)
                    return new AddMemberServiceResponse(
                         AddMemberServiceResponseStatus.EmployeeNotExists);

                project.ProjectMembers.Add(new()
                {
                    Role = ProjectMemberRoles.Member,
                    OrganizationEmployeeId = employee.Id
                });
                 await _context.SaveChangesAsync();

                return new AddMemberServiceResponse(
                     AddMemberServiceResponseStatus.Success);
            }
            catch (Exception ex)
            {
                _logger.LogError("AddMemberService : {Message}", ex.Message);

                return new AddMemberServiceResponse(
                     AddMemberServiceResponseStatus.InternalError);
            }
        }
    }
}
