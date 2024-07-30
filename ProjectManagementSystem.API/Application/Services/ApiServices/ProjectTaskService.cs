using Domain.Constants.AuthorizationResponses;
using Domain.Constants.Roles.OrganiationEmployees;
using Domain.Models.ApiModels.ProjectTask.Request;
using Domain.Models.ServiceResponses.Organization;
using Domain.Models.ServiceResponses.ProjectTask;
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
    public class ProjectTaskService(DataContext context,
        ILogger<ProjectTaskService> logger,
        IAuthorizationService authService,
        IRealTimeTaskService realTimeTaskService) : IProjectTaskService
    {
        private readonly DataContext _context = context;
        private readonly ILogger _logger = logger;
        private readonly IAuthorizationService _authService = authService;
        private readonly IRealTimeTaskService _realTimeTaskService = realTimeTaskService;

        /// <summary>
        /// Changes Task priority
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns>ChangeProjectTaskPriorityServiceResponse</returns>
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

                var authResult = await _authService
                    .AuthorizeByProjectId(taskList.ProjectId, userId,
                        [ProjectMemberRoles.Member]);

                if (authResult.Equals(AuthorizationResponse.Deny))
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
                _logger.LogError("ChangeTaskPriorityService : {Message}", ex.Message);

                return new ChangeProjectTaskPriorityServiceResponse(
                     ChangeProjectTaskPriorityServiceResponseStatus.InternalError);
            }
        }

        /// <summary>
        /// Creates Task in TaskList
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns>CreateProjectTaskServiceResponse</returns>
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

                var authResult = await _authService
                   .AuthorizeByProjectId(taskList.ProjectId, userId,
                       [ProjectMemberRoles.Member]);

                if (authResult.Equals(AuthorizationResponse.Deny))
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
                            .Select(t => t.Id).FirstOrDefaultAsync();

                return new CreateProjectTaskServiceResponse(
                     CreateProjectTaskServiceResponseStatus.Success)
                {
                    NewTask = new()
                    {
                        Id = id.ToString(),
                        Title = request.Name,
                        Description = request.Description,
                        Priority = lastPriority
                    }
                };

            }
            catch (Exception ex)
            {
                _logger.LogError("CreateTaskService : {Message}", ex.Message);

                return new CreateProjectTaskServiceResponse(
                     CreateProjectTaskServiceResponseStatus.InternalError);
            }

        }

        /// <summary>
        /// Removes Task from TaskList
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns>DeleteProjectTaskServiceResponse</returns>
        public async Task<DeleteProjectTaskServiceResponse> DeleteTask(DeleteProjectTaskRequest request, string userId)
        {
            try
            {
                var task = await _context.ProjectTasks
                    .FirstOrDefaultAsync(t => t.Id.ToString().Equals(request.TaskId));

                if (task == null)
                    return new DeleteProjectTaskServiceResponse(
                         DeleteProjectTaskServiceResponseStatus.TaskNotExists);

                var taskList = await _context.ProjectTaskLists
                    .Include(tl => tl.ProjectTasks)
                        .FirstOrDefaultAsync(tl => tl.Id
                             .Equals(task.ProjectTaskListId));

                var authResult = await _authService
                    .AuthorizeByProjectId(taskList.ProjectId, userId,
                        [ProjectMemberRoles.Member]);

                if (authResult.Equals(AuthorizationResponse.Deny))
                    return new DeleteProjectTaskServiceResponse(
                         DeleteProjectTaskServiceResponseStatus.AccessDenied);

                _context.ProjectTasks.Remove(task);

                foreach (var t in taskList.ProjectTasks)
                {
                    if (t.Priority > task.Priority)
                        --t.Priority;
                }

                await _context.SaveChangesAsync();

                return new DeleteProjectTaskServiceResponse(
                     DeleteProjectTaskServiceResponseStatus.Success);

            }
            catch (Exception ex)
            {
                logger.LogError("DeleteTaskService : {Message}", ex.Message);

                return new DeleteProjectTaskServiceResponse(
                     DeleteProjectTaskServiceResponseStatus.InternalError);
            }

        }

        /// <summary>
        /// Used for getting Task details
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns>GetProjectTaskServiceResponse</returns>
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

                var authResult = await _authService
                    .AuthorizeByProjectId(taskList.ProjectId, userId);

                if (authResult.Equals(AuthorizationResponse.Deny))
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
                _logger.LogError("GetTaskService : {Message}", ex.Message);

                return new GetProjectTaskServiceResponse(
                     GetProjectTaskServiceResponseStatus.InternalError);
            }
        }

        /// <summary>
        /// Updates Task details
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<UpdateProjectTaskServiceResponse> UpdateTask(UpdateProjectTaskRequest request, string userId)
        {
            try
            {
                var task = await _context.ProjectTasks
                    .FirstOrDefaultAsync(t => t.Id.ToString().Equals(request.TaskId));

                if (task == null)
                    return new UpdateProjectTaskServiceResponse(
                         UpdateProjectTaskServiceResponseStatus.TaskNotExists);

                var taskList = await _context.ProjectTaskLists.AsNoTracking()
                    .Include(tl => tl.ProjectTasks)
                        .FirstOrDefaultAsync(tl => tl.Id
                             .Equals(task.ProjectTaskListId));

                var authResult = await _authService
                    .AuthorizeByProjectId(taskList.ProjectId, userId,
                        [ProjectMemberRoles.Member]);

                if (authResult.Equals(AuthorizationResponse.Deny))
                    return new UpdateProjectTaskServiceResponse(
                         UpdateProjectTaskServiceResponseStatus.AccessDenied);

                task.Title = request.Title;
                task.Description = request.Description;

                await _context.SaveChangesAsync();

                return new UpdateProjectTaskServiceResponse(
                     UpdateProjectTaskServiceResponseStatus.Success);
            }
            catch (Exception ex)
            {
                _logger.LogError("UpdateTaskService : {Message}", ex.Message);

                return new UpdateProjectTaskServiceResponse(
                     UpdateProjectTaskServiceResponseStatus.InternalError);
            }
        }

        /// <summary>
        /// Changes Task`s TaskList
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ChangeProjectTasksTaskListServiceResponse> ChangeTaskList(ChangeProjectTasksTaskListRequest request, string userId)
        {
            try
            {
                var task = await _context.ProjectTasks
                    .FirstOrDefaultAsync(t => t.Id.ToString()
                        .Equals(request.TaskId));

                if (task == null)
                    return new ChangeProjectTasksTaskListServiceResponse(
                         ChangeProjectTasksTaskListServiceResponseStatus.TaskNotExists);

                var taskList = await _context.ProjectTaskLists
                    .Include(tl => tl.ProjectTasks)
                        .FirstOrDefaultAsync(tl => tl.Id
                             .Equals(task.ProjectTaskListId));

                var authResult = await _authService
                    .AuthorizeByProjectId(taskList.ProjectId, userId,
                        [ProjectMemberRoles.Member]);

                if (authResult.Equals(AuthorizationResponse.Deny))
                    return new ChangeProjectTasksTaskListServiceResponse(
                         ChangeProjectTasksTaskListServiceResponseStatus.AccessDenied);

                var targetTaskList = await _context.ProjectTaskLists.Include(t => t.ProjectTasks)
                    .FirstOrDefaultAsync(tl => tl.Id.ToString().Equals(request.TargetTaskListId));

                if (targetTaskList == null)
                    return new ChangeProjectTasksTaskListServiceResponse(
                         ChangeProjectTasksTaskListServiceResponseStatus.TaskListNotExists);

                foreach (var t in targetTaskList.ProjectTasks)
                {
                    if (t.Priority >= request.TargetPriority)
                    {
                        t.Priority += 1;
                    }
                }

                task.ProjectTaskListId = targetTaskList.Id;

                await _context.Entry(taskList)
                    .Collection(tl => tl.ProjectTasks).LoadAsync();

                foreach (var t in taskList.ProjectTasks)
                {
                    if (t.Priority > task.Priority)
                    {
                        t.Priority -= 1;
                    }
                }

                task.Priority = request.TargetPriority;

                await _context.SaveChangesAsync();

                await _realTimeTaskService
                    .SendChangeTaskList(new()
                {
                    TaskId=task.Id.ToString(),
                    TargetTaskListId = targetTaskList.Id.ToString(),
                    TargetPriority=request.TargetPriority

                },taskList.ProjectId.ToString());

                return new ChangeProjectTasksTaskListServiceResponse(
                     ChangeProjectTasksTaskListServiceResponseStatus.Success);
            }
            catch (Exception ex)
            {
                _logger.LogError("UpdateTaskService : {Message}", ex.Message);

                return new ChangeProjectTasksTaskListServiceResponse(
                     ChangeProjectTasksTaskListServiceResponseStatus.InternalError);
            }
        }
    }
}
