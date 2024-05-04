using Domain.Constants.AuthorizationResponses;
using Domain.Constants.Roles.OrganiationEmployees;
using Domain.Models.ApiModels.Project.Request;
using Domain.Models.ServiceResponses.Project;
using Domain.Models.ServiceResponses.ProjectTask;
using Domain.Services.ApiServices;
using Domain.Services.InternalServices;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.ApiServices
{
    public class ProjectService(DataContext context,
        ILogger<ProjectService> logger,
            IAuthorizationService authService) : IProjectService
    {

        private readonly DataContext _context = context;
        private readonly ILogger<ProjectService> _logger = logger;
        private readonly IAuthorizationService _authService = authService;

        public async Task<CreateProjectServiceResponse> CreateProject(CreateProjectRequest request, string userId)
        {
            try
            {
                var org = await _context.Organizations
                    .Include(o => o.Projects).ThenInclude(p => p.ProjectMembers)
                       .Include(o => o.OrganizationEmployees)
                          .FirstOrDefaultAsync(o => o.Id.ToString()
                              .Equals(request.OrganizationId));

                if (org == null)
                    return new CreateProjectServiceResponse(
                         CreateProjectServiceResponseStatus.OrganizationNotExists);

                var authResult = await _authService
                    .AuthorizeByOrganizationId(org.Id, userId,
                         [OrganizationEmployeesRoles.Member]);

                if (authResult.Equals(AuthorizationResponse.Deny))
                    return new CreateProjectServiceResponse(
                         CreateProjectServiceResponseStatus.AccessDenied);

                var employee = org.OrganizationEmployees
                    .FirstOrDefault(e => e.UserId.Equals(userId));

                org.Projects.Add(new()
                {
                    Name = request.Name,
                    Description = request.Description,
                    Status = "proccesing",
                    LeaderId = employee.Id.ToString(),
                    Creationlevel = "Pending"
                });

                await _context.SaveChangesAsync();

                var project = await _context.Projects
                    .Include(p => p.ProjectMembers)
                        .FirstOrDefaultAsync(p => p.LeaderId
                            .Equals(employee.Id.ToString()) &&
                                p.Creationlevel.Equals("Pending"));

                project.ProjectMembers.Add(new()
                {
                    Role = ProjectMemberRoles.Leader,
                    OrganizationEmployeeId = employee.Id,
                    ProjectId = project.Id
                });

                project.Creationlevel = "Final";

                await _context.SaveChangesAsync();

                return new CreateProjectServiceResponse(
                     CreateProjectServiceResponseStatus.Success);
            }
            catch (Exception ex)
            {
                _logger.LogError("CreateProjectService : {Message}", ex.Message);

                return new CreateProjectServiceResponse(
                     CreateProjectServiceResponseStatus.InternalError);
            }
        }

        public async Task<DeleteProjectServiceResponse> DeleteProject(DeleteProjectRequest request, string userId)
        {
            try
            {
                var project = await _context.Projects
                    .AsNoTracking().Include(p => p.ProjectTaskLists)
                        .ThenInclude(tl => tl.ProjectTasks)
                            .FirstOrDefaultAsync(p => p.Id.ToString()
                                .Equals(request.ProjectId));

                if (project == null)
                    return new DeleteProjectServiceResponse(
                         DeleteProjectServiceResponseStatus.ProjectNotExists);

                var members = await _context.ProjectMembers
                   .AsNoTracking().Where(m => m.ProjectId
                       .Equals(project.Id)).ToListAsync();

                var authResult = await _authService
                   .AuthorizeByProjectId(project.Id, userId,
                       [ProjectMemberRoles.Member]);

                if (authResult.Equals(AuthorizationResponse.Deny))
                    return new DeleteProjectServiceResponse(
                         DeleteProjectServiceResponseStatus.AccessDenied);

                _context.ProjectMembers.RemoveRange(members);

                _context.Projects.Remove(project);

                await _context.SaveChangesAsync();

                return new DeleteProjectServiceResponse(
                     DeleteProjectServiceResponseStatus.Success);
            }
            catch (Exception ex)
            {
                _logger.LogError("DeleteProjectService : {Message}", ex.Message);

                return new DeleteProjectServiceResponse(
                     DeleteProjectServiceResponseStatus.InternalError);
            }
        }

        public async Task<GetProjectServiceResponse> GetProject(GetProjectRequest request, string userId)
        {
            try
            {
                var project = await _context.Projects
                    .Include(p => p.ProjectMembers)
                        .ThenInclude(pm => pm.OrganizationEmployee)
                            .ThenInclude(e => e.User)
                                .Include(p => p.ProjectTaskLists)
                                    .ThenInclude(tl => tl.ProjectTasks)
                                        .AsNoTracking().FirstOrDefaultAsync
                                            (p => p.Id.ToString().Equals(request.ProjectId));

                if (project == null)
                    return new GetProjectServiceResponse(
                         GetProjectServiceResponseStatus.ProjectNotExists);

                var authResult = await _authService
                   .AuthorizeByOrganizationId(project.OrganizationId, userId);

                if (authResult.Equals(AuthorizationResponse.Deny))
                    return new GetProjectServiceResponse(
                         GetProjectServiceResponseStatus.AccessDenied);

                return new GetProjectServiceResponse(
                     GetProjectServiceResponseStatus.Success)
                {
                    Project = project
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("GetProjectService : {Message}", ex.Message);

                return new GetProjectServiceResponse(
                     GetProjectServiceResponseStatus.InternalError);
            }
        }

        public async Task<ChangeProjectLeaderServiceResponse> ChangeLeadr(ChangeProjectLeaderRequest request, string userId)
        {
            try
            {
                var project = await _context.Projects
                .Include(p => p.ProjectMembers)
                    .FirstOrDefaultAsync(p => p.Id.ToString()
                        .Equals(request.ProjectId));

                if (project == null)
                    return new ChangeProjectLeaderServiceResponse(
                         ChangeProjectLeaderServiceResponseStatus.ProjectNotExists);

                var authResult = await _authService
                    .AuthorizeByOrganizationId(project.OrganizationId, userId,
                        [OrganizationEmployeesRoles.Member,
                            OrganizationEmployeesRoles.Admin]);

                if (authResult.Equals(AuthorizationResponse.Deny))
                    return new ChangeProjectLeaderServiceResponse(
                         ChangeProjectLeaderServiceResponseStatus.AccessDenied);

                var oldLeadr = project.ProjectMembers
                    .FirstOrDefault(p => p.Role
                        .Equals(ProjectMemberRoles.Leader));

                var newLeader = project.ProjectMembers
                    .FirstOrDefault(m => m.Id.ToString()
                        .Equals(request.NewLeaderMemberId));

                if (newLeader == null)
                    return new ChangeProjectLeaderServiceResponse(
                         ChangeProjectLeaderServiceResponseStatus.LeaderNotExists);

                newLeader.Role = ProjectMemberRoles.Leader;

                project.LeaderId = newLeader
                    .OrganizationEmployeeId.ToString();

                oldLeadr.Role = ProjectMemberRoles.Admin;

                await _context.SaveChangesAsync();

                return new ChangeProjectLeaderServiceResponse(
                     ChangeProjectLeaderServiceResponseStatus.Success);
            }
            catch (Exception ex)
            {
                _logger.LogError("ChangeProjectLeadrService : {Message}", ex.Message);

                return new ChangeProjectLeaderServiceResponse(
                     ChangeProjectLeaderServiceResponseStatus.InternalError);
            }
        }
    }
}
