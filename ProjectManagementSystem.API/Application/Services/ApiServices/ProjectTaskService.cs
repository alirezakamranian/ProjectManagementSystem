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
    ILogger<ProjectTaskService> logger) : IProjectTaskService
    {
        private readonly DataContext _context = context;
        private readonly ILogger<ProjectTaskService> _logger = logger;

        public async Task<ChangeProjectTaskPriorityServiceResponse> ChangePriority(ChangeProjectTaskPriorityRequest request, string userId)
        {
            try
            {
                var task = await _context.ProjectTasks
                    .FirstOrDefaultAsync(t => t.Id.ToString()
                         .Equals(request.TaskId));

                if (task == null)
                    return new ChangeProjectTaskPriorityServiceResponse(
                         ChangeProjectTaskPriorityServiceResponseStatus.TaskNotExists);

                var taskList = await _context.ProjectTaskLists
                    .FirstOrDefaultAsync(tl => tl.Id.Equals(task.ProjectTaskListId));

                var project = await _context.Projects.AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Id.Equals(taskList.ProjectId));

                var org = await _context.Organizations
                    .Include(o => o.OrganizationEmployees).AsNoTracking()
                        .FirstOrDefaultAsync(o => o.Id.Equals(project.OrganizationId));

                var employee = org.OrganizationEmployees
                    .Where(e => e.UserId.Equals(userId)).FirstOrDefault();

                if (employee == null)
                    return new ChangeProjectTaskPriorityServiceResponse(
                        ChangeProjectTaskPriorityServiceResponseStatus.AccessDenied);

                await _context.Entry(project)
                    .Collection(p => p.ProjectMembers).LoadAsync();

                if (!project.ProjectMembers.Any(p =>
                       p.OrganizationEmployeeId.Equals(employee.Id) && (
                            p.Role.Equals(ProjectMemberRoles.Leader) ||
                                 p.Role.Equals(ProjectMemberRoles.Admin) ||
                                      p.Role.Equals(ProjectMemberRoles.Modrator))))
                    return new ChangeProjectTaskPriorityServiceResponse(
                         ChangeProjectTaskPriorityServiceResponseStatus.AccessDenied);

                await _context.Entry(taskList)
                    .Collection(tl => tl.ProjectTasks).LoadAsync();

                var targetTask = taskList.ProjectTasks
                    .FirstOrDefault(t => t.Priority.Equals(request.NewPriority));

                if (targetTask == null)
                    return new ChangeProjectTaskPriorityServiceResponse(
                         ChangeProjectTaskPriorityServiceResponseStatus.InvalidPriority);

                targetTask.Priority = task.Priority;
                task.Priority = request.NewPriority;

                await _context.SaveChangesAsync();

                return new ChangeProjectTaskPriorityServiceResponse(
                     ChangeProjectTaskPriorityServiceResponseStatus.Success);
            }
            catch (Exception ex)
            {
                _logger.LogError("ChangeProjectTaskPriorityService : {Message}", ex.Message);

                return new ChangeProjectTaskPriorityServiceResponse(
                     ChangeProjectTaskPriorityServiceResponseStatus.InternalError);
            }
        }

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

                var project = await _context.Projects.AsNoTracking()
                     .FirstOrDefaultAsync(p => p.Id.Equals(taskList.ProjectId));

                var org = await _context.Organizations.Include(o => o.OrganizationEmployees)
                    .AsNoTracking().FirstOrDefaultAsync(o => o.Id
                        .Equals(project.OrganizationId));

                var employee = org.OrganizationEmployees.Where(e =>
                            e.UserId.Equals(userId)).FirstOrDefault();

                if (employee == null)
                    return new CreateProjectTaskServiceResponse(
                         CreateProjectTaskServiceResponseStatus.AccessDenied);

                await _context.Entry(project)
                   .Collection(p => p.ProjectMembers).LoadAsync();

                if (!project.ProjectMembers.Any(p =>
                    p.OrganizationEmployeeId.Equals(employee.Id) && (
                         p.Role.Equals(ProjectMemberRoles.Leader) ||
                              p.Role.Equals(ProjectMemberRoles.Admin) ||
                                   p.Role.Equals(ProjectMemberRoles.Modrator))))
                    return new CreateProjectTaskServiceResponse(
                         CreateProjectTaskServiceResponseStatus.AccessDenied);

                var lastPriority = taskList.ProjectTasks
                    .Select(t => t.Priority).DefaultIfEmpty(0).Max();

                taskList.ProjectTasks.Add(new()
                {
                    Title = request.Name,
                    Description = request.Description,
                    Priority = ++lastPriority
                });

                await _context.SaveChangesAsync();

                var id = await _context.ProjectTasks
                    .AsNoTracking().Where(t => t.ProjectTaskListId
                        .Equals(taskList.Id) && t.Priority == lastPriority)
                            .Select(t=>t.Id).FirstOrDefaultAsync();

                return new CreateProjectTaskServiceResponse(
                     CreateProjectTaskServiceResponseStatus.Success)
                {
                    NewTask = new()
                    {
                        Id=id.ToString(),
                        Title=request.Name,
                        Description = request.Description,
                        Priority= lastPriority
                    }
                };

            }
            catch (Exception ex)
            {
                _logger.LogError("CreateProjectTaskService : {Message}", ex.Message);

                return new CreateProjectTaskServiceResponse(
                     CreateProjectTaskServiceResponseStatus.InternalError);
            }

        }

        public async Task<DeleteProjectTaskServiceResponse> DeleteTask(DeleteProjectTaskRequest request, string userId)
        {
            try
            {
                var task = await _context.ProjectTasks
                    .FirstOrDefaultAsync(t => t.Id.ToString() == request.TaskId);
                if (task == null)
                    return new DeleteProjectTaskServiceResponse(
                         DeleteProjectTaskServiceResponseStatus.TaskNotExists);

                var taskList = await _context.ProjectTaskLists.AsNoTracking()
                    .Include(tl => tl.ProjectTasks)
                        .FirstOrDefaultAsync(tl => tl.Id
                             .Equals(task.ProjectTaskListId));

                var project = await _context.Projects.AsNoTracking()
                      .FirstOrDefaultAsync(p => p.Id.Equals(taskList.ProjectId));

                var org = await _context.Organizations
                    .Include(o => o.OrganizationEmployees)
                        .AsNoTracking()
                            .FirstOrDefaultAsync(o => o.Id
                                .Equals(project.OrganizationId));

                var employee = org.OrganizationEmployees
                    .FirstOrDefault(e => e.UserId.Equals(userId));

                if (employee == null)
                    return new DeleteProjectTaskServiceResponse(
                         DeleteProjectTaskServiceResponseStatus.AccessDenied);

                await _context.Entry(project)
                  .Collection(p => p.ProjectMembers).LoadAsync();

                if (!project.ProjectMembers.Any(p =>
                   p.OrganizationEmployeeId.Equals(employee.Id) && (
                        p.Role.Equals(ProjectMemberRoles.Leader) ||
                             p.Role.Equals(ProjectMemberRoles.Admin) ||
                                  p.Role.Equals(ProjectMemberRoles.Modrator))))
                    return new DeleteProjectTaskServiceResponse(
                         DeleteProjectTaskServiceResponseStatus.AccessDenied);


                _context.ProjectTasks.Remove(task);

                await _context.SaveChangesAsync();

                return new DeleteProjectTaskServiceResponse(
                     DeleteProjectTaskServiceResponseStatus.Success);

            }
            catch (Exception ex)
            {
                logger.LogError("DeleteTaskTaskService : {Message}", ex.Message);

                return new DeleteProjectTaskServiceResponse(
                     DeleteProjectTaskServiceResponseStatus.InternalError);
            }

        }

        public async Task<GetProjectTaskServiceResponse> GetTask(GetProjectTaskRequest request, string userId)
        {
            try
            {
                var task = await _context.ProjectTasks
                    .FirstOrDefaultAsync(t => t.Id.ToString()
                         .Equals(request.TaskId));

                if (task == null)
                    return new GetProjectTaskServiceResponse(
                         GetProjectTaskServiceResponseStatus.TaskNotExists);

                var taskList = await _context.ProjectTaskLists.AsNoTracking()
                    .Include(tl => tl.ProjectTasks)
                        .FirstOrDefaultAsync(tl => tl.Id
                             .Equals(task.ProjectTaskListId));

                var project = await _context.Projects
                    .AsNoTracking().Include(p => p.ProjectMembers)
                        .FirstOrDefaultAsync(p => p.Id.Equals(taskList.ProjectId));

                var org = await _context.Organizations.Include(o => o.OrganizationEmployees)
                    .AsNoTracking().FirstOrDefaultAsync(o => o.Id
                        .Equals(project.OrganizationId));

                var employee = org.OrganizationEmployees.Where(e =>
                    e.UserId.Equals(userId)).FirstOrDefault();

                if (employee == null)
                    return new GetProjectTaskServiceResponse(
                         GetProjectTaskServiceResponseStatus.AccessDenied);

                if (!project.ProjectMembers.Any(p =>
                    p.OrganizationEmployeeId.Equals(employee.Id)))
                    return new GetProjectTaskServiceResponse(
                         GetProjectTaskServiceResponseStatus.AccessDenied);

                return new GetProjectTaskServiceResponse(
                     GetProjectTaskServiceResponseStatus.Success)
                {
                    Task = task
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("GetProjectTaskService : {Message}", ex.Message);

                return new GetProjectTaskServiceResponse(
                     GetProjectTaskServiceResponseStatus.InternalError);
            }
        }
    }
}
