using Azure.Core;
using Domain.Constants.AuthorizationResponses;
using Domain.Constants.Roles.OrganiationEmployees;
using Domain.Entities.Project;
using Domain.Models.ApiModels.TaskLableManagement.Request;
using Domain.Models.Dtos.Task;
using Domain.Models.ServiceResponses.TaskComment;
using Domain.Models.ServiceResponses.TaskLabelManagement;
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
    public class TaskLableManagementService(DataContext context,
        ILogger<ProjectTaskService> logger,
        IAuthorizationService authService) : ITaskLableManagementService
    {
        private readonly DataContext _context = context;
        private readonly ILogger _logger = logger;
        private readonly IAuthorizationService _authService = authService;

        public async Task<CreateTaskLabelServiceResponse> CreateTaskLabel(CreateTaskLabelRequest request, string userId)
        {
            try
            {
                var project = await _context.Projects
                    .FirstOrDefaultAsync(p => p.Id.ToString()
                        .Equals(request.ProjectId));

                if (project == null)
                    return new CreateTaskLabelServiceResponse(
                         CreateTaskLabelServiceResponseStatus.ProjectNotExists);

                var authResult = await _authService
                    .AuthorizeByProjectId(project.Id,
                        userId, [ProjectMemberRoles.Member]);

                if (authResult.Equals(AuthorizationResponse.Deny))
                    return new CreateTaskLabelServiceResponse(
                         CreateTaskLabelServiceResponseStatus.AccessDenied);

                await _context.Entry(project)
                    .Collection(p => p.Lables).LoadAsync();

                project.Lables.Add(new()
                {
                    Title = request.Title,
                    Color = request.Color,
                });

                await _context.SaveChangesAsync();

                return new CreateTaskLabelServiceResponse(
                     CreateTaskLabelServiceResponseStatus.Success);
            }
            catch (Exception ex)
            {
                _logger.LogError("CreateTaskLabelService : {Message}", ex.Message);

                return new CreateTaskLabelServiceResponse(
                     CreateTaskLabelServiceResponseStatus.InternalError);
            }
        }

        public async Task<GetProjectTaskLabelsServiceResponse> GetProjectTaskLabels(GetProjectTaskLabelsRequset request, string userId)
        {
            try
            {
                var project = await _context.Projects
                    .FirstOrDefaultAsync(p => p.Id.ToString()
                        .Equals(request.ProjectId));

                if (project == null)
                    return new GetProjectTaskLabelsServiceResponse(
                         GetProjectTaskLabelsServiceResponseStatus.ProjectNotExists);

                var authResult = await _authService
                    .AuthorizeByProjectId(project.Id, userId);

                if (authResult.Equals(AuthorizationResponse.Deny))
                    return new GetProjectTaskLabelsServiceResponse(
                         GetProjectTaskLabelsServiceResponseStatus.AccessDenied);

                await _context.Entry(project)
                    .Collection(p => p.Lables).LoadAsync();

                List<TaskLabelForResponseDto> labels = [];

                foreach (var l in project.Lables)
                {
                    labels.Add(new()
                    {
                        Id=l.Id.ToString(),
                        Title = l.Title,
                        ColorCode = l.Color
                    });
                }

                return new GetProjectTaskLabelsServiceResponse(
                     GetProjectTaskLabelsServiceResponseStatus.Success)
                {
                    Labels = labels
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("GetProjectTaskLabelsService : {Message}", ex.Message);

                return new GetProjectTaskLabelsServiceResponse(
                     GetProjectTaskLabelsServiceResponseStatus.InternalError);
            }
        }

        public async Task<RemoveTaskLabelServiceResponse> RemoveTaskLabel(RemoveTaskLabelRequest request, string userId)
        {
            try
            {
                var label = await _context.TaskLables
               .FirstOrDefaultAsync(l => l.Id.ToString()
                   .Equals(request.LabelId));

                if (label == null)
                    return new RemoveTaskLabelServiceResponse(
                         RemoveTaskLabelServiceResponseStatus.LabelNotExists);

                var authResult = await _authService
                        .AuthorizeByProjectId(label.ProjectId,
                            userId, [ProjectMemberRoles.Member]);

                if (authResult.Equals(AuthorizationResponse.Deny))
                    return new RemoveTaskLabelServiceResponse(
                         RemoveTaskLabelServiceResponseStatus.AccessDenied);

                _context.TaskLables.Remove(label);

                await _context.SaveChangesAsync();

                return new RemoveTaskLabelServiceResponse(
                     RemoveTaskLabelServiceResponseStatus.Success);
            }
            catch (Exception ex)
            {
                _logger.LogError("RemoveTaskLabelService : {Message}", ex.Message);

                return new RemoveTaskLabelServiceResponse(
                     RemoveTaskLabelServiceResponseStatus.InternalError);
            }
        }

        public async Task<UpdateTaskLabelServiceResponse> UpdateTaskLabel(UpdateTaskLabelRequest request, string userId)
        {
            try
            {
                var label = await _context.TaskLables
                    .FirstOrDefaultAsync(l => l.Id.ToString()
                        .Equals(request.LabelId));

                if (label == null)
                    return new UpdateTaskLabelServiceResponse(
                         UpdateTaskLabelServiceResponseStatus.LabelNotExists);

                var authResult = await _authService
                    .AuthorizeByProjectId(label.ProjectId,
                        userId, [ProjectMemberRoles.Member]);

                if (authResult.Equals(AuthorizationResponse.Deny))
                    return new UpdateTaskLabelServiceResponse(
                         UpdateTaskLabelServiceResponseStatus.AccessDenied);

                label.Title = request.NewTitle;
                label.Color = request.NewTitle;

                await _context.SaveChangesAsync();

                return new UpdateTaskLabelServiceResponse(
                     UpdateTaskLabelServiceResponseStatus.Success);
            }
            catch (Exception ex)
            {
                _logger.LogError("UpdateTaskLabelService : {Message}", ex.Message);

                return new UpdateTaskLabelServiceResponse(
                    UpdateTaskLabelServiceResponseStatus.InternalError);
            }
        }
    }
}
