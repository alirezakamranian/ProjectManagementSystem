using Domain.Constants.AuthorizationResponses;
using Domain.Constants.Roles.OrganiationEmployees;
using Domain.Entities.Project.ProjectTask;
using Domain.Models.ApiModels.TaskAssignment.Request;
using Domain.Models.ServiceResponses.ProjectTask;
using Domain.Models.ServiceResponses.TaskAssignment;
using Domain.Services.ApiServices;
using Domain.Services.InternalServices;
using Infrastructure.DataAccess;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.ApiServices
{
    public class TaskAssignmentService(DataContext context,
        ILogger<TaskAssignmentService> logger,
        IAuthorizationService authService) : ITaskAssignmentService
    {
        private readonly DataContext _context = context;
        private readonly ILogger<TaskAssignmentService> _logger = logger;
        private readonly IAuthorizationService _authService = authService;

        public async Task<AssignTaskServiceResponse> AssignTask(AssignTaskRequest request, string userId)
        {
            try
            {
                var task = await _context.ProjectTasks
                .FirstOrDefaultAsync(t => t.Id.ToString()
                    .Equals(request.TaskId));

                if (task == null)
                    return new AssignTaskServiceResponse(
                         AssignTaskServiceResponseStatus.TaskNotExists);

                var taskList = await _context.ProjectTaskLists
                    .AsNoTracking().FirstOrDefaultAsync(tl => tl.Id
                        .Equals(task.ProjectTaskListId));

                var authResult = await _authService.AuthorizeByProjectId(
                    taskList.ProjectId, userId, [ProjectMemberRoles.Member]);

                if (authResult.Equals(AuthorizationResponse.Deny))
                    return new AssignTaskServiceResponse(
                         AssignTaskServiceResponseStatus.AccessDenied);

                var member = await _context.ProjectMembers.AsNoTracking()
                    .Where(pm => pm.ProjectId.Equals(taskList.ProjectId))
                        .FirstOrDefaultAsync(pm => pm.Id.ToString().Equals(request.MemberId));

                if (member == null)
                    return new AssignTaskServiceResponse(
                          AssignTaskServiceResponseStatus.MemberNotExists);

                task.Assignment = new()
                {
                    Description = request.Description,
                    ProjectMemberId = member.Id
                };

                await _context.SaveChangesAsync();

                return new AssignTaskServiceResponse(
                     AssignTaskServiceResponseStatus.Success);
            }
            catch (Exception ex)
            {
                _logger.LogError("AssignTaskService : {Message}", ex.Message);

                return new AssignTaskServiceResponse(
                     AssignTaskServiceResponseStatus.InternalError);
            }
        }

        public async Task<RemoveTaskAssignmentServiceResponse> RemoveAssignment(RemoveAssignmentRequest request, string userId)
        {
            try
            {
                var task = await _context.ProjectTasks.Include(t => t.Assignment)
                    .FirstOrDefaultAsync(t => t.Id.ToString()
                        .Equals(request.TaskId));

                if (task == null)
                    return new RemoveTaskAssignmentServiceResponse(
                         RemoveTaskAssignmentServiceResponseStatus.TaskNotExists);

                if (task.Assignment == null)
                    return new RemoveTaskAssignmentServiceResponse(
                         RemoveTaskAssignmentServiceResponseStatus.AssignmentNotExists);

                var taskList = await _context.ProjectTaskLists
                    .AsNoTracking().FirstOrDefaultAsync(tl => tl.Id
                        .Equals(task.ProjectTaskListId));

                var authResult = await _authService.AuthorizeByProjectId(
                    taskList.ProjectId, userId, [ProjectMemberRoles.Member]);

                if (authResult.Equals(AuthorizationResponse.Deny))
                    return new RemoveTaskAssignmentServiceResponse(
                         RemoveTaskAssignmentServiceResponseStatus.AccessDenied);

                _context.Remove(task.Assignment);

                await _context.SaveChangesAsync();

                return new RemoveTaskAssignmentServiceResponse(
                     RemoveTaskAssignmentServiceResponseStatus.Success);
            }
            catch (Exception ex)
            {
                _logger.LogError("RemoveAssignmentService : {Message}", ex.Message);

                return new RemoveTaskAssignmentServiceResponse(
                     RemoveTaskAssignmentServiceResponseStatus.InternalError);
            }
        }
    }
}
