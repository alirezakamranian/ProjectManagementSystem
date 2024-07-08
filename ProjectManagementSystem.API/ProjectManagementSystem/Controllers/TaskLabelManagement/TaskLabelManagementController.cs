using Azure.Core;
using Domain.Entities.Project;
using Domain.Models.ApiModels.TaskComment.Response;
using Domain.Models.ApiModels.TaskLableManagement.Request;
using Domain.Models.ApiModels.TaskLableManagement.Response;
using Domain.Models.ServiceResponses.TaskComment;
using Domain.Models.ServiceResponses.TaskLabelManagement;
using Domain.Services.ApiServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProjectManagementSystem.Controllers.TaskLabelManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskLabelManagementController(ITaskLableManagementService taskLableManagementService) : ControllerBase
    {

        private readonly ITaskLableManagementService _taskLableManagementService = taskLableManagementService;

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateLabel(CreateTaskLabelRequest request)
        {
            var userId = HttpContext.User.Claims
               .FirstOrDefault(c => c.Type.Equals("Id")).Value;

            var serviceResponse = await _taskLableManagementService
                .CreateTaskLabel(request, userId);

            if (serviceResponse.Status.Equals(CreateTaskLabelServiceResponseStatus.AccessDenied) ||
               serviceResponse.Status.Equals(CreateTaskLabelServiceResponseStatus.ProjectNotExists))
                return StatusCode(StatusCodes.Status400BadRequest,
                       new CreateTaskLabelResponse
                       {
                           Message = serviceResponse.Status
                       });

            if (serviceResponse.Status.Equals(CreateTaskLabelServiceResponseStatus.InternalError))
                return StatusCode(StatusCodes.Status500InternalServerError,
                        new CreateTaskLabelResponse
                        {
                            Message = serviceResponse.Status
                        });

            return Ok(new CreateTaskLabelResponse
            {
                Message = serviceResponse.Status
            });
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> RemoveLabel(RemoveTaskLabelRequest request)
        {
            var userId = HttpContext.User.Claims
              .FirstOrDefault(c => c.Type.Equals("Id")).Value;

            var serviceResponse = await _taskLableManagementService
                .RemoveTaskLabel(request, userId);

            if (serviceResponse.Status.Equals(RemoveTaskLabelServiceResponseStatus.AccessDenied) ||
               serviceResponse.Status.Equals(RemoveTaskLabelServiceResponseStatus.LabelNotExists))
                return StatusCode(StatusCodes.Status400BadRequest,
                       new RemoveTaskLabelResponse
                       {
                           Message = serviceResponse.Status
                       });

            if (serviceResponse.Status.Equals(RemoveTaskLabelServiceResponseStatus.InternalError))
                return StatusCode(StatusCodes.Status500InternalServerError,
                        new RemoveTaskLabelResponse
                        {
                            Message = serviceResponse.Status
                        });

            return Ok(new RemoveTaskLabelResponse
            {
                Message = serviceResponse.Status
            });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetLabels([FromQuery] string projectId)
        {
            var userId = HttpContext.User.Claims
             .FirstOrDefault(c => c.Type.Equals("Id")).Value;

            var serviceResponse = await _taskLableManagementService
                .GetProjectTaskLabels(new() { ProjectId = projectId }, userId);

            if (serviceResponse.Status.Equals(GetProjectTaskLabelsServiceResponseStatus.AccessDenied) ||
               serviceResponse.Status.Equals(GetProjectTaskLabelsServiceResponseStatus.ProjectNotExists))
                return StatusCode(StatusCodes.Status400BadRequest,
                       new GetProjectTaskLabelsResponse
                       {
                           Message = serviceResponse.Status
                       });

            if (serviceResponse.Status.Equals(GetProjectTaskLabelsServiceResponseStatus.InternalError))
                return StatusCode(StatusCodes.Status500InternalServerError,
                        new GetProjectTaskLabelsResponse
                        {
                            Message = serviceResponse.Status
                        });

            return Ok(new GetProjectTaskLabelsResponse
            {
                Message = serviceResponse.Status,
                Labels = serviceResponse.Labels
            });
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateLabel(UpdateTaskLabelRequest request)
        {
            var userId = HttpContext.User.Claims
            .FirstOrDefault(c => c.Type.Equals("Id")).Value;

            var serviceResponse = await _taskLableManagementService
                .UpdateTaskLabel(request, userId);

            if (serviceResponse.Status.Equals(UpdateTaskLabelServiceResponseStatus.AccessDenied) ||
               serviceResponse.Status.Equals(UpdateTaskLabelServiceResponseStatus.LabelNotExists))
                return StatusCode(StatusCodes.Status400BadRequest,
                       new UpdateTaskLabelResponse
                       {
                           Message = serviceResponse.Status
                       });

            if (serviceResponse.Status.Equals(UpdateTaskLabelServiceResponseStatus.InternalError))
                return StatusCode(StatusCodes.Status500InternalServerError,
                        new UpdateTaskLabelResponse
                        {
                            Message = serviceResponse.Status
                        });

            return Ok(new UpdateTaskLabelResponse
            {
                Message = serviceResponse.Status
            });
        }
    }
}
