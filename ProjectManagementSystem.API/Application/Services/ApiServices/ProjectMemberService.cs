using Domain.Models.ServiceResponses.ProjectMember;
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

        /// <summary>
        /// Adds OrgEmployee to project
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns>AddMemberServiceResponse</returns>
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
                     AddMemberServiceResponseStatus.Success)
                {
                    Member = await _context.ProjectMembers
                        .Include(m => m.OrganizationEmployee)
                            .ThenInclude(e => e.User).AsNoTracking()
                                .FirstOrDefaultAsync(pm => pm.OrganizationEmployeeId
                                    .Equals(employee.Id))
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("AddProjectMemberService : {Message}", ex.Message);

                return new AddMemberServiceResponse(
                     AddMemberServiceResponseStatus.InternalError);
            }
        }

        /// <summary>
        /// Changes Member role
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ChangeProjectMemberRoleServiceResponse> ChangeMemberRole(ChangeProjectMemberRoleRequest request, string userId)
        {
            try
            {
                if(request.NewRole.Equals(ProjectMemberRoles.Leader))
                    return new ChangeProjectMemberRoleServiceResponse(
                         ChangeProjectMemberRoleServiceResponseStatus.LeaderRoleCanNotChanged);

                var project = await _context.Projects
                    .AsNoTracking().FirstOrDefaultAsync(
                        p => p.Id.ToString().Equals(request.ProjectId));

                if (project == null)
                    return new ChangeProjectMemberRoleServiceResponse(
                         ChangeProjectMemberRoleServiceResponseStatus.ProjectNotExists);

                var authResult = await _authService
                    .AuthorizeByProjectId(project.Id, userId,
                         [ProjectMemberRoles.Member,
                      ProjectMemberRoles.Admin]);

                if (authResult.Equals(AuthorizationResponse.Deny))
                    return new ChangeProjectMemberRoleServiceResponse(
                         ChangeProjectMemberRoleServiceResponseStatus.AccessDenied);

                var member = await _context.ProjectMembers
                    .FirstOrDefaultAsync(m => m.Id.ToString()
                        .Equals(request.MemberId));

                if (member == null)
                    return new ChangeProjectMemberRoleServiceResponse(
                         ChangeProjectMemberRoleServiceResponseStatus.MemberNotExists);

                member.Role = request.NewRole;

                await _context.SaveChangesAsync();

                return new ChangeProjectMemberRoleServiceResponse(
                     ChangeProjectMemberRoleServiceResponseStatus.Success);

            }
            catch (Exception ex)
            {
                _logger.LogError("ChangeProjectMemberRoleService : {Message}", ex.Message);

                return new ChangeProjectMemberRoleServiceResponse(
                     ChangeProjectMemberRoleServiceResponseStatus.InternalError);
            }
        }

        /// <summary>
        /// Removes OrgEmployee from project
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns>RemoveProjectMemberServiceResponse</returns>
        public async Task<RemoveProjectMemberServiceResponse> RemoveMember(RemoveProjectMemberRequest request, string userId)
        {
            try
            {
                var project = await _context.Projects
                .Include(p => p.ProjectMembers).AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Id
                        .ToString().Equals(request.ProjectId));

                if (project == null)
                    return new RemoveProjectMemberServiceResponse(
                         RemoveProjectMemberServiceResponseStatus.ProjectNotExists);

                var authResult = await _authService
                       .AuthorizeByProjectId(project.Id, userId,
                           [ProjectMemberRoles.Member,
                           ProjectMemberRoles.Admin]);

                if (authResult.Equals(AuthorizationResponse.Deny))
                    return new RemoveProjectMemberServiceResponse(
                         RemoveProjectMemberServiceResponseStatus.AccessDenied);

                var member = project.ProjectMembers
                    .FirstOrDefault(m => m.Id.ToString()
                        .Equals(request.MemberId));

                if (member == null)
                    return new RemoveProjectMemberServiceResponse(
                         RemoveProjectMemberServiceResponseStatus.MemberNotExists);

                if (member.Role.Equals(ProjectMemberRoles.Leader))
                    return new RemoveProjectMemberServiceResponse(
                         RemoveProjectMemberServiceResponseStatus.LeaderCanNotRemoved);

                _context.ProjectMembers.Remove(member);

                await _context.SaveChangesAsync();

                return new RemoveProjectMemberServiceResponse(
                     RemoveProjectMemberServiceResponseStatus.Success);

            }
            catch (Exception ex)
            {
                _logger.LogError("RemoveProjectMemberService : {Message}", ex.Message);

                return new RemoveProjectMemberServiceResponse(
                     RemoveProjectMemberServiceResponseStatus.InternalError);
            }
        }
    }
}
