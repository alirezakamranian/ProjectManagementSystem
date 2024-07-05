using Domain.Constants.AuthorizationResponses;
using Domain.Constants.Roles.OrganiationEmployees;
using Domain.Models.ApiModels.TaskComment.Request;
using Domain.Models.Dtos.Task;
using Domain.Models.ServiceResponses.Project;
using Domain.Models.ServiceResponses.TaskComment;
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
    public class TaskCommentService(DataContext context,
        ILogger<ProjectTaskService> logger,
        IAuthorizationService authService) : ITaskCommentService
    {
        private readonly DataContext _context = context;
        private readonly ILogger _logger = logger;
        private readonly IAuthorizationService _authService = authService;

        public async Task<AddTaskCommentServiceResponse> AddComment(AddTaskCommentRequest request, string userId)
        {
            try
            {
                var task = await _context.ProjectTasks
                    .FirstOrDefaultAsync(t => t.Id.ToString()
                        .Equals(request.TaskId));

                if (task == null)
                    return new AddTaskCommentServiceResponse(
                         AddTaskCommentServiceResponseStatus.TaskNotExists);

                await _context.Entry(task).Reference(
                    t => t.ProjectTaskList).LoadAsync();

                var authResult = await _authService
                    .AuthorizeByProjectId(task.ProjectTaskList.ProjectId,
                        userId, [ProjectMemberRoles.Member]);

                if (authResult.Equals(AuthorizationResponse.Deny))
                    return new AddTaskCommentServiceResponse(
                         AddTaskCommentServiceResponseStatus.AccessDenied);

                var project = await _context.Projects
                   .Select(p => new { p.OrganizationId, p.Id })
                       .FirstOrDefaultAsync(p => p.Id.Equals(
                           task.ProjectTaskList.ProjectId));

                var employee = await _context.OrganizationEmployees
                    .Select(e => new { e.OrganizationId, e.Id })
                        .FirstOrDefaultAsync(e => e.OrganizationId
                            .Equals(project.OrganizationId));

                var member = await _context.ProjectMembers
                    .Select(pm => new { pm.Id, pm.OrganizationEmployeeId, pm.ProjectId })
                        .FirstOrDefaultAsync(pm => pm.OrganizationEmployeeId
                            .Equals(employee.Id) && pm.ProjectId.Equals(project.Id));

                await _context.Entry(task).Collection(
                    t => t.Comments).LoadAsync();

                task.Comments.Add(new()
                {
                    Text = request.Text,
                    MemberId = member.Id,
                    TaskId = task.Id
                });

                await _context.SaveChangesAsync();

                return new AddTaskCommentServiceResponse(
                     AddTaskCommentServiceResponseStatus.Success);
            }
            catch (Exception ex)
            {
                _logger.LogError("AddTaskCommentService : {Message}", ex.Message);

                return new AddTaskCommentServiceResponse(
                     AddTaskCommentServiceResponseStatus.InternalError);
            }
        }

        public async Task<GetTaskCommentServiceResponse> GetTaskComments(GetTaskCommentsRequest request, string userId)
        {
            try
            {
                var task = await _context.ProjectTasks
                  .FirstOrDefaultAsync(t => t.Id.ToString()
                      .Equals(request.TaskId));

                if (task == null)
                    return new GetTaskCommentServiceResponse(
                         GetTaskCommentServiceResponseStatus.TaskNotExists);

                await _context.Entry(task).Reference(t => t.ProjectTaskList).LoadAsync();

                var authResult = await _authService.AuthorizeByProjectId(
                    task.ProjectTaskList.ProjectId, userId);

                if (authResult.Equals(AuthorizationResponse.Deny))
                    return new GetTaskCommentServiceResponse(
                         GetTaskCommentServiceResponseStatus.AccessDenied);

                await _context.Entry(task).Collection(
                     t => t.Comments).LoadAsync();

                List<TaskCommentForResponseDto> comments = [];

                foreach (var c in task.Comments)
                {
                    comments.Add(new()
                    {
                        MemberId = c.MemberId.ToString(),
                        TaskId = c.TaskId.ToString(),
                        Text = c.Text,
                        Id = c.Id.ToString()
                    });
                }

                return new GetTaskCommentServiceResponse(
                     GetTaskCommentServiceResponseStatus.Success)
                {
                    Comments = comments
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("GetTaskCommentService : {Message}", ex.Message);

                return new GetTaskCommentServiceResponse(
                     GetTaskCommentServiceResponseStatus.InternalError);
            }
        }

        public async Task<RemoveTaskCommentServiceResponse> RemoveTaskComment(RemoveTaskCommentRequest request, string userId)
        {
            try
            {
                var comment = await _context.TaskComments
                    .FirstOrDefaultAsync(c => c.Id.ToString()
                        .Equals(request.CommentId));

                if (comment == null)
                    return new RemoveTaskCommentServiceResponse(
                         RemoveTaskCommentServiceResponseStatus.CommentNotExists);

                var task = await _context.ProjectTasks
                    .FirstOrDefaultAsync(t => t.Id
                        .Equals(comment.TaskId));

                await _context.Entry(task).Reference(
                    t => t.ProjectTaskList).LoadAsync();

                var authResult = await _authService.AuthorizeByProjectId(
                    task.ProjectTaskList.ProjectId, userId);

                if (authResult.Equals(AuthorizationResponse.Deny))
                    return new RemoveTaskCommentServiceResponse(
                         RemoveTaskCommentServiceResponseStatus.AccessDenied);

                _context.TaskComments.Remove(comment);

                await _context.SaveChangesAsync();

                return new RemoveTaskCommentServiceResponse(
                     RemoveTaskCommentServiceResponseStatus.Success);
            }
            catch (Exception ex)
            {
                _logger.LogError("RemoveTaskCommentService : {Message}", ex.Message);

                return new RemoveTaskCommentServiceResponse(
                     RemoveTaskCommentServiceResponseStatus.InternalError);
            }
        }

        public async Task<UpdateTaskCommentServiceResponse> UpdateTaskComment(UpdateTaskCommentRequest request, string userId)
        {
            try
            {
                var comment = await _context.TaskComments
                  .FirstOrDefaultAsync(c => c.Id.ToString()
                      .Equals(request.CommentId));

                if (comment == null)
                    return new UpdateTaskCommentServiceResponse(
                         UpdateTaskCommentServiceResponseStatus.CommentNotExists);

                var task = await _context.ProjectTasks
                    .FirstOrDefaultAsync(t => t.Id
                        .Equals(comment.TaskId));

                await _context.Entry(task).Reference(
                    t => t.ProjectTaskList).LoadAsync();

                var authResult = await _authService.AuthorizeByProjectId(
                    task.ProjectTaskList.ProjectId, userId);

                if (authResult.Equals(AuthorizationResponse.Deny))
                    return new UpdateTaskCommentServiceResponse(
                        UpdateTaskCommentServiceResponseStatus.AccessDenied);

                comment.Text = request.NewText;

                await _context.SaveChangesAsync();

                return new UpdateTaskCommentServiceResponse(
                     UpdateTaskCommentServiceResponseStatus.Success);
            }
            catch (Exception ex)
            {
                _logger.LogError("UpdateTaskCommentService : {Message}", ex.Message);

                return new UpdateTaskCommentServiceResponse(
                     UpdateTaskCommentServiceResponseStatus.InternalError);
            }
        }
    }
}
