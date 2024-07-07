using Domain.Constants.AuthorizationResponses;
using Domain.Constants.Roles.OrganiationEmployees;
using Domain.Models.ApiModels.TaskLabelAttachment.Request;
using Domain.Models.ServiceResponses.TaskAssignment;
using Domain.Models.ServiceResponses.TaskLabelAttachment;
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
    public class TaskLabelAttachmentService(DataContext context,
        ILogger<TaskAssignmentService> logger,
        IAuthorizationService authService) : ITaskLabelAttachmentService
    {
        private readonly DataContext _context = context;
        private readonly ILogger<TaskAssignmentService> _logger = logger;
        private readonly IAuthorizationService _authService = authService;

        public async Task<AttachTaskLabelServiceResponse> AttachTaskLabel(AttachTaskLabelRequest request, string userId)
        {
            try
            {
                var task = await _context.ProjectTasks
                    .FirstOrDefaultAsync(t => t.Id.ToString()
                        .Equals(request.TaskId));

                if (task == null)
                    return new AttachTaskLabelServiceResponse(
                        AttachTaskLabelServiceResponseStatus.TaskNotExists);

                var taskList = await _context.ProjectTaskLists
                    .AsNoTracking().FirstOrDefaultAsync(tl => tl.Id
                        .Equals(task.ProjectTaskListId));

                var authResult = await _authService.AuthorizeByProjectId(
                    taskList.ProjectId, userId, [ProjectMemberRoles.Member]);

                if (authResult.Equals(AuthorizationResponse.Deny))
                    return new AttachTaskLabelServiceResponse(
                         AttachTaskLabelServiceResponseStatus.AccessDenied);

                await _context.Entry(task).Reference(
                    t => t.LabelAttachment).LoadAsync();

                if (task.LabelAttachment != null)
                    return new AttachTaskLabelServiceResponse(
                         AttachTaskLabelServiceResponseStatus.TaskAlredyHasLabel);

                task.LabelAttachment = new()
                {
                    LabelId = Guid.Parse(request.LabelId)
                };

                await _context.SaveChangesAsync();

                return new AttachTaskLabelServiceResponse(
                     AttachTaskLabelServiceResponseStatus.Success);
            }
            catch (Exception ex)
            {
                _logger.LogError("AttachTaskLabelService : {Message}", ex.Message);

                return new AttachTaskLabelServiceResponse(
                     AttachTaskLabelServiceResponseStatus.InternalError);
            }
        }

        public async Task<RemoveTaskLabelAttachmentServiceResponse> RemoveTaskLabelAttachment(RemoveTaskLabelAttachmentRequest request, string userId)
        {
            try
            {
                var task = await _context.ProjectTasks
                    .FirstOrDefaultAsync(t => t.Id.ToString()
                        .Equals(request.TaskId));

                if (task == null)
                    return new RemoveTaskLabelAttachmentServiceResponse(
                         RemoveTaskLabelAttachmentServiceResponseStatus.TaskNotExists);

                var taskList = await _context.ProjectTaskLists
                    .AsNoTracking().FirstOrDefaultAsync(tl => tl.Id
                        .Equals(task.ProjectTaskListId));

                var authResult = await _authService.AuthorizeByProjectId(
                    taskList.ProjectId, userId, [ProjectMemberRoles.Member]);

                if (authResult.Equals(AuthorizationResponse.Deny))
                    return new RemoveTaskLabelAttachmentServiceResponse(
                         RemoveTaskLabelAttachmentServiceResponseStatus.AccessDenied);

                await _context.Entry(task).Reference(
                    t => t.LabelAttachment).LoadAsync();

                _context.TaskLabelAttachments.Remove(task.LabelAttachment);

                await _context.SaveChangesAsync();

                return new RemoveTaskLabelAttachmentServiceResponse(
                     RemoveTaskLabelAttachmentServiceResponseStatus.Success);
            }
            catch (Exception ex)
            {
                _logger.LogError("RemoveTaskLabelAttachmentService : {Message}", ex.Message);

                return new RemoveTaskLabelAttachmentServiceResponse(
                     RemoveTaskLabelAttachmentServiceResponseStatus.InternalError);
            }
        }
    }
}
