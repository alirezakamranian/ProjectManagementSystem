using Amazon.S3.Model;
using Domain.Constants.AuthorizationResponses;
using Domain.Constants.Roles.OrganiationEmployees;
using Domain.Models.ApiModels.Project.Request;
using Domain.Models.Dtos.Project;
using Domain.Models.ServiceResponses.Project;
using Domain.Models.ServiceResponses.ProjectTask;
using Domain.Models.ServiceResponses.Storage;
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
        IAuthorizationService authService,
        IStorageService storageService) : IProjectService
    {

        private readonly DataContext _context = context;
        private readonly ILogger<ProjectService> _logger = logger;
        private readonly IAuthorizationService _authService = authService;
        private readonly IStorageService _storageService = storageService;

        /// <summary>
        /// Creates project in organization
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns>CreateProjectServiceResponse</returns>
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
                     CreateProjectServiceResponseStatus.Success)
                {
                    ProjectId = project.Id.ToString()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("CreateProjectService : {Message}", ex.Message);

                return new CreateProjectServiceResponse(
                     CreateProjectServiceResponseStatus.InternalError);
            }
        }

        /// <summary>
        /// Removes Project from organization
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Used for get project with all TaskList,Task and members
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns>GetProjectServiceResponse</returns>
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
                                        .ThenInclude(t=>t.Assignment)
                                            .AsNoTracking().FirstOrDefaultAsync
                                                (p => p.Id.ToString().Equals(request.ProjectId));

                if (project == null)
                    return new GetProjectServiceResponse(
                         GetProjectServiceResponseStatus.ProjectNotExists);

                var authResult = await _authService
                   .AuthorizeByProjectId(project.Id, userId);

                if (authResult.Equals(AuthorizationResponse.Deny))
                    return new GetProjectServiceResponse(
                         GetProjectServiceResponseStatus.AccessDenied);

                var getUrlResponse = await _storageService.GetUrl(new()
                {
                    FileKey = project.Id.ToString()
                });

                List<ProjectTaskListForResponseDto> taskLists = [];

                foreach (var tl in project.ProjectTaskLists)
                {
                    var tasks = new List<ProjectTaskForResponseDto>();

                    foreach (var t in tl.ProjectTasks)
                    {
                        var task = new ProjectTaskForResponseDto()
                        {
                            Id = t.Id.ToString(),
                            Title = t.Title,
                            Description = t.Description,
                            Priority = t.Priority,
                        };

                        if (t.Assignment != null)
                            task.Assignment = new()
                            {
                                Id = t.Assignment.Id.ToString(),
                                MemberId = t.Assignment.ProjectMemberId.ToString(),
                                Description = t.Assignment.Description
                            };

                        tasks.Add(task);
                    }

                    taskLists.Add(new()
                    {
                        Id = tl.Id.ToString(),
                        Name = tl.Name,
                        Priority = tl.Priority,
                        Tasks = tasks.OrderBy(t => t.Priority).ToList()
                    });
                }

                List<ProjectMemberForResponseDto> members = [];

                foreach (var m in project.ProjectMembers)
                {
                    var getMembersAvatarUrlResponse = await _storageService
                        .GetUrl(new() { FileKey = m.OrganizationEmployee.User.Id });

                    members.Add(new()
                    {
                        Id = m.Id.ToString(),
                        UserId = m.OrganizationEmployee.User.Id,
                        Name = m.OrganizationEmployee.User.FullName,
                        Role = m.Role,
                        AvatarUrl = getMembersAvatarUrlResponse.Url
                    });
                }

                return new GetProjectServiceResponse(
                     GetProjectServiceResponseStatus.Success)
                {
                    Project = new()
                    {
                        Id = project.Id.ToString(),
                        Name = project.Name,
                        Description = project.Description,
                        AvatarLink = getUrlResponse.Url,
                        Status = project.Status,
                        Members = members,
                        ProjectTaskLists = taskLists.OrderBy(tl => tl.Priority).ToList()
                    },
                    AvatarUrl = getUrlResponse.Url
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("GetProjectService : {Message}", ex.Message);

                return new GetProjectServiceResponse(
                     GetProjectServiceResponseStatus.InternalError);
            }
        }

        /// <summary>
        /// Changes Leader of project
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns>ChangeProjectLeaderServiceResponse</returns>
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
