using Domain.Constants.Roles.OrganiationEmployees;
using Domain.Entities.HumanResource;
using Domain.Models.Dtos.ProjectTask.Request;
using Domain.Models.Dtos.ProjectTaskList.Request;
using Domain.Models.ServiceResponses.ProjectMember;
using Domain.Models.ServiceResponses.ProjectTaskList;
using Domain.Services.ApiServices;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.ApiServices
{
    public class ProjectTaskListService(DataContext context,
        ILogger<AuthenticationService> logger) : IProjectTaskListService
    {
        private readonly DataContext _context = context;
        private readonly ILogger<AuthenticationService> _logger = logger;

        public async Task<ChangeTaskListPriorityServiceResponse> ChangeTaskListPriority(ChangeTaskListPriorityRequest request, string userId)
        {
            try
            {
                var currentTaskList = await _context.ProjectTaskLists
                    .FirstOrDefaultAsync(tl => tl.Id.ToString()
                        .Equals(request.TaskListId));

                if (currentTaskList == null)
                    return new ChangeTaskListPriorityServiceResponse(
                         ChangeTaskListPriorityServiceResponseStatus.TaskListNotExists);

                var project = _context.Projects.Include(p => p.ProjectMembers)
                    .FirstOrDefault(p => p.Id.Equals(currentTaskList.ProjectId));

                var org = await _context.Organizations.AsNoTracking()
                    .Include(o => o.OrganizationEmployees)
                        .FirstOrDefaultAsync(o => o.Id
                            .Equals(project.OrganizationId));

                var employee = org.OrganizationEmployees
                    .FirstOrDefault(e => e.UserId.Equals(userId));

                if (employee == null)
                    return new ChangeTaskListPriorityServiceResponse(
                         ChangeTaskListPriorityServiceResponseStatus.AccessDenied);

                if (!project.ProjectMembers.Any(m =>
                    m.OrganizationEmployeeId.Equals(employee.Id) &&
                        (m.Role.Equals(ProjectMemberRoles.Leader) ||
                            m.Role.Equals(ProjectMemberRoles.Admin) ||
                                m.Role.Equals(ProjectMemberRoles.Modrator))))
                    return new ChangeTaskListPriorityServiceResponse(
                         ChangeTaskListPriorityServiceResponseStatus.AccessDenied);

                var targetTaskList = await _context.ProjectTaskLists
                    .FirstOrDefaultAsync(tl => tl.Priority
                        .Equals(request.NewPriority) && tl.ProjectId.Equals(project.Id));

                if (targetTaskList == null)
                    return new ChangeTaskListPriorityServiceResponse(
                         ChangeTaskListPriorityServiceResponseStatus.InvalidPriority);

                targetTaskList.Priority = currentTaskList.Priority;
                currentTaskList.Priority = request.NewPriority;

                await _context.SaveChangesAsync();

                return new ChangeTaskListPriorityServiceResponse(
                     ChangeTaskListPriorityServiceResponseStatus.Success);
            }
            catch (Exception ex)
            {
                _logger.LogError("ChangeTaskListPriorityService : {Message}", ex.Message);

                return new ChangeTaskListPriorityServiceResponse(
                     ChangeTaskListPriorityServiceResponseStatus.InternalError);
            }

        }

        public async Task<ProjectTaskListServiceResponse> CreateTaskList(CreateTaskListRequest request,string userId)
        {
            try
            {
                var project = await _context.Projects
                    .Include(p => p.ProjectMembers)
                        .Include(p => p.ProjectTaskLists)
                            .FirstOrDefaultAsync(p =>
                                 p.Id.ToString().Equals(request.ProjectId));

                if (project == null)
                    return new ProjectTaskListServiceResponse(
                         ProjectTaskListServiceResponseStatus.ProjectNotExists);

                var org = await _context.Organizations.AsNoTracking()
                    .Include(o => o.OrganizationEmployees)
                        .FirstOrDefaultAsync(o => o.Id
                            .Equals(project.OrganizationId));

                var employee = org.OrganizationEmployees
                    .FirstOrDefault(e => e.UserId.Equals(userId));

                if (employee == null)
                    return new ProjectTaskListServiceResponse(
                         ProjectTaskListServiceResponseStatus.AccessDenied);

                if (!project.ProjectMembers.Any(m =>
                   m.OrganizationEmployeeId.Equals(employee.Id) &&
                       (m.Role.Equals(ProjectMemberRoles.Leader) ||
                           m.Role.Equals(ProjectMemberRoles.Admin) ||
                               m.Role.Equals(ProjectMemberRoles.Modrator))))
                    return new ProjectTaskListServiceResponse(
                         ProjectTaskListServiceResponseStatus.AccessDenied);

                var lastPriority = project.ProjectTaskLists
                    .Select(t => t.Priority)
                        .DefaultIfEmpty(0).Max();

                project.ProjectTaskLists.Add(new()
                {
                    Name = request.Name,
                    Priority = (++lastPriority)
                });

                await _context.SaveChangesAsync();

                var taskListId = await _context.ProjectTaskLists
                    .Where(tl => tl.ProjectId.Equals(project.Id) && 
                        tl.Priority.Equals(lastPriority))
                            .Select(tl => tl.Id).FirstAsync();

                return new ProjectTaskListServiceResponse(
                     ProjectTaskListServiceResponseStatus.Success)
                {
                    TaskList = new()
                    {
                        Id = taskListId.ToString(),
                        Name = request.Name,
                        Priority = lastPriority
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("CreateTaskListService : {Message}", ex.Message);

                return new ProjectTaskListServiceResponse(
                     ProjectTaskListServiceResponseStatus.InternalError);
            }
        }

        public async Task<DeleteTaskListServiceResponse> DeleteTaskList(DeleteTaskListRequest request, string userId)
        {
            try
            {
                var taskList = await _context.ProjectTaskLists
                    .FirstOrDefaultAsync(tl => tl.Id.ToString()
                        .Equals(request.TaskListId));

                if (taskList == null)
                    return new DeleteTaskListServiceResponse(
                         DeleteTaskListServiceResponseStatus.TaskListNotExists);

                var project = _context.Projects
                    .AsNoTracking().Include(p => p.ProjectMembers)
                        .FirstOrDefault(p => p.Id.Equals(taskList.ProjectId));

                var org = await _context.Organizations.AsNoTracking()
                    .Include(o => o.OrganizationEmployees)
                        .FirstOrDefaultAsync(o => o.Id
                            .Equals(project.OrganizationId));

                var employee = org.OrganizationEmployees
                    .FirstOrDefault(e => e.UserId.Equals(userId));

                if(employee == null)
                    return new DeleteTaskListServiceResponse(
                        DeleteTaskListServiceResponseStatus.AccessDenied);

                if (!project.ProjectMembers.Any(m =>
                   m.OrganizationEmployeeId.Equals(employee.Id) &&
                       (m.Role.Equals(ProjectMemberRoles.Leader) ||
                           m.Role.Equals(ProjectMemberRoles.Admin) ||
                               m.Role.Equals(ProjectMemberRoles.Modrator))))
                    return new DeleteTaskListServiceResponse(
                         DeleteTaskListServiceResponseStatus.AccessDenied);

                _context.ProjectTaskLists.Remove(taskList);

                await _context.SaveChangesAsync();

                return new DeleteTaskListServiceResponse(
                     DeleteTaskListServiceResponseStatus.Success);
            }
            catch (Exception ex)
            {
                _logger.LogError("DeleteTaskListService : {Message}", ex.Message);

                return new DeleteTaskListServiceResponse(
                     DeleteTaskListServiceResponseStatus.InternalError);
            }
        }

        public async Task<UpdateTaskListServiceResponse> UpdateTaskList(UpdateTaskListRequest request, string userId)
        {
            try
            {
                var taskList = await _context.ProjectTaskLists
               .FirstOrDefaultAsync(tl => tl.Id.ToString()
                   .Equals(request.TaskListId));

                if (taskList == null)
                    return new UpdateTaskListServiceResponse(
                        UpdateTaskListServiceResponseStatus.TaskListNotExists);

                var project = _context.Projects
                    .AsNoTracking().Include(p => p.ProjectMembers)
                        .FirstOrDefault(p => p.Id.Equals(taskList.ProjectId));

                var org = await _context.Organizations.AsNoTracking()
                    .Include(o => o.OrganizationEmployees)
                        .FirstOrDefaultAsync(o => o.Id
                            .Equals(project.OrganizationId));

                var employee = org.OrganizationEmployees
                    .FirstOrDefault(e => e.UserId.Equals(userId));

                if (employee == null)
                    return new UpdateTaskListServiceResponse(
                          UpdateTaskListServiceResponseStatus.AccessDenied);

                if (!project.ProjectMembers.Any(m =>
                   m.OrganizationEmployeeId.Equals(employee.Id) &&
                       (m.Role.Equals(ProjectMemberRoles.Leader) ||
                           m.Role.Equals(ProjectMemberRoles.Admin) ||
                               m.Role.Equals(ProjectMemberRoles.Modrator))))
                    return new UpdateTaskListServiceResponse(
                         UpdateTaskListServiceResponseStatus.AccessDenied);

                taskList.Name = request.Title;

                await _context.SaveChangesAsync();

                return new UpdateTaskListServiceResponse(
                     UpdateTaskListServiceResponseStatus.Success);
            }
            catch (Exception ex)
            {
                _logger.LogError("UpdateTaskListService : {Message}", ex.Message);

                return new UpdateTaskListServiceResponse(
                     UpdateTaskListServiceResponseStatus.InternalError);
            }
        }
    }
}
