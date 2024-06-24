using Azure.Core;
using Domain.Models.ApiModels.Organization.Response;
using Domain.Models.ApiModels.TaskAssignment.Request;
using Domain.Models.ApiModels.TaskAssignment.Response;
using Domain.Models.ServiceResponses.Organization;
using Domain.Models.ServiceResponses.TaskAssignment;
using Domain.Services.ApiServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProjectManagementSystem.Controllers.TaskAssignment
{
    [Route("TaskAssignment")]
    [ApiController]
    public class TaskAssignmentController(ITaskAssignmentService assignmentService) : ControllerBase
    {
        private readonly ITaskAssignmentService _assignmentService = assignmentService;

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AssignTask(AssignTaskRequest request)
        {
            var userId = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type.Equals("Id")).Value;

            var serviceResponse = await _assignmentService
                .AssignTask(request, userId);


            if (serviceResponse.Status.Equals(AssignTaskServiceResponseStatus.MemberNotExists) ||
                serviceResponse.Status.Equals(AssignTaskServiceResponseStatus.AccessDenied)||
                serviceResponse.Status.Equals(AssignTaskServiceResponseStatus.TaskNotExists))
                return StatusCode(StatusCodes.Status400BadRequest,
                        new AssignTaskResponse
                        {
                            Message = serviceResponse.Status
                        });

            if (serviceResponse.Status.Equals(AssignTaskServiceResponseStatus.InternalError))
                return StatusCode(StatusCodes.Status500InternalServerError,
                        new AssignTaskResponse
                        {
                            Message = serviceResponse.Status
                        });

            return Ok(new AssignTaskResponse
            {
                Message = serviceResponse.Status
            });
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Remove([FromQuery] string taskId)
        {
            var userId = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type.Equals("Id")).Value;

            var serviceResponse = await _assignmentService
                .RemoveAssignment(new() {TaskId = taskId }, userId);

            if (serviceResponse.Status.Equals(RemoveTaskAssignmentServiceResponseStatus.AccessDenied) ||
               serviceResponse.Status.Equals(RemoveTaskAssignmentServiceResponseStatus.TaskNotExists))
                return StatusCode(StatusCodes.Status400BadRequest,
                        new RemoveAssignmentResponse
                        {
                            Message = serviceResponse.Status
                        });

            if (serviceResponse.Status.Equals(RemoveTaskAssignmentServiceResponseStatus.InternalError))
                return StatusCode(StatusCodes.Status500InternalServerError,
                        new RemoveAssignmentResponse
                        {
                            Message = serviceResponse.Status
                        });

            return Ok(new RemoveAssignmentResponse
            {
                Message = serviceResponse.Status
            });
        }
    }
}
