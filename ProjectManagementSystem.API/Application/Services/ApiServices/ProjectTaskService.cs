using Domain.Constants.Roles.OrganiationEmployees;
using Domain.Models.Dtos.ProjectTask.Request;
using Domain.Models.ServiceResponses.Organization;
using Domain.Models.ServiceResponses.ProjectTask;
using Domain.Services.ApiServices;
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
    public class ProjectTaskService(DataContext context,
    ILogger<AuthenticationService> logger) : IProjectTaskService
    {
        private readonly DataContext _context = context;
        private readonly ILogger<AuthenticationService> _logger = logger;

        public async Task<CreateProjectTaskServiceResponse> CreateTask(CreateProjectTaskRequest request, string userId)
        {
            try
            {
                var taskList = await _context.ProjectTaskLists
                .Include(tl => tl.ProjectTasks)
                    .FirstOrDefaultAsync(tl => tl.Id.ToString()
                         .Equals(request.TaskListId));

                if (taskList == null)
                    return new CreateProjectTaskServiceResponse(
                         CreateProjectTaskServiceResponseStatus.TaskListNotExists);

                var project = await _context.Projects
                    .AsNoTracking().Include(p => p.ProjectMembers)
                        .FirstOrDefaultAsync(p => p.Id.Equals(taskList.ProjectId));

                var org = await _context.Organizations.Include(o => o.OrganizationEmployees)
                    .AsNoTracking().FirstOrDefaultAsync(o => o.Id
                        .Equals(project.OrganizationId));

                if (!project.ProjectMembers.Any(p =>
                    p.OrganizationEmployeeId.Equals(
                        org.OrganizationEmployees.Where(e => 
                            e.UserId.Equals(userId)).Select(e => e.Id)) && (
                                p.Role.Equals(ProjectMemberRoles.Leader) ||
                                    p.Role.Equals(ProjectMemberRoles.Admin) ||
                                        p.Role.Equals(ProjectMemberRoles.Modrator))))
                    return new CreateProjectTaskServiceResponse(
                         CreateProjectTaskServiceResponseStatus.AccessDenied);

                taskList.ProjectTasks.Add(new()
                {
                    Title = request.Name,
                    Description = request.Description,
                    Priority = taskList.ProjectTasks.Count + 1
                });

                await _context.SaveChangesAsync();

                return new CreateProjectTaskServiceResponse(
                     CreateProjectTaskServiceResponseStatus.Success);

            }
            catch (Exception ex)
            {
                _logger.LogError("CreateProjectTaskService : {Message}", ex.Message);

                return new CreateProjectTaskServiceResponse(
                     CreateProjectTaskServiceResponseStatus.InternalError);
            }

        }
    }
}
