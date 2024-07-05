using Domain.Constants.AuthorizationResponses;
using Domain.Constants.Roles.OrganiationEmployees;
using Domain.Models.ApiModels.TaskLableManagement.Request;
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
                _logger.LogError("CreateTaskLabelServiceService : {Message}", ex.Message);

                return new CreateTaskLabelServiceResponse(
                     CreateTaskLabelServiceResponseStatus.InternalError);
            }
        }
    }
}
