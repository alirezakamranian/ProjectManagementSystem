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
    }
}
