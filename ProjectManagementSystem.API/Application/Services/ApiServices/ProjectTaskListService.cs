using Domain.Constants.Roles.OrganiationEmployees;
using Domain.Entities.HumanResource;
using Domain.Models.ApiModels.ProjectTaskList.Request;
using Domain.Models.ApiModels.ProjectTask.Request;
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
using Domain.Services.InternalServices;
using Domain.Constants.AuthorizationResponses;
using Microsoft.Identity.Client;
using System.Diagnostics;

namespace Application.Services.ApiServices
{
    public class ProjectTaskListService(DataContext context,
        ILogger<ProjectTaskListService> logger,
        IAuthorizationService authService) : IProjectTaskListService
    {
        private readonly DataContext _context = context;
        private readonly ILogger<ProjectTaskListService> _logger = logger;
        private readonly IAuthorizationService _authService = authService;

        /// <summary>
        /// Changes TaskList priority
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns>ChangeTaskListPriorityServiceResponse</returns>
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

                var authResult = await _authService
                     .AuthorizeByProjectId(project.Id, userId,
                         [ProjectMemberRoles.Member]);

                if (authResult.Equals(AuthorizationResponse.Deny))
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

        /// <summary>
        /// Creates TaskList in project
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns>ProjectTaskListServiceResponse</returns>
        public async Task<ProjectTaskListServiceResponse> CreateTaskList(CreateTaskListRequest request, string userId)
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

                var authResult = await _authService
                     .AuthorizeByProjectId(project.Id, userId,
                         [ProjectMemberRoles.Member]);

                if (authResult.Equals(AuthorizationResponse.Deny))
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

        /// <summary>
        /// Removes TaskList from project
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns>DeleteTaskListServiceResponse</returns>
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

                var authResult = await _authService
                    .AuthorizeByProjectId(project.Id, userId,
                        [ProjectMemberRoles.Member]);

                if (authResult.Equals(AuthorizationResponse.Deny))
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

        /// <summary>
        /// Updates TaskList details
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns>UpdateTaskListServiceResponse</returns>
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

                var authResult = await _authService
                    .AuthorizeByProjectId(project.Id, userId,
                        [ProjectMemberRoles.Member]);

                if (authResult.Equals(AuthorizationResponse.Deny))
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
