using Application.Services.ApiServices;
using Domain.Models.ApiModels.TaskLabelAttachment.Request;
using Domain.Models.ApiModels.TaskLabelAttachment.Response;
using Domain.Models.ApiModels.TaskLableManagement.Response;
using Domain.Models.ServiceResponses.TaskLabelAttachment;
using Domain.Models.ServiceResponses.TaskLabelManagement;
using Domain.Services.ApiServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProjectManagementSystem.Controllers.TaskLabelAttachment
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskLabelAttachmentController(ITaskLabelAttachmentService taskLabelAttachmentService) : ControllerBase
    {
        private readonly ITaskLabelAttachmentService _taskLabelAttachmentService = taskLabelAttachmentService;

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AttachLabel(AttachTaskLabelRequest request)
        {
            var userId = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type.Equals("Id")).Value;

            var serviceResponse = await _taskLabelAttachmentService
                .AttachTaskLabel(request, userId);

            if (serviceResponse.Status.Equals(AttachTaskLabelServiceResponseStatus.AccessDenied) ||
               serviceResponse.Status.Equals(AttachTaskLabelServiceResponseStatus.TaskNotExists)||
               serviceResponse.Status.Equals(AttachTaskLabelServiceResponseStatus.LabelNotExists)||
               serviceResponse.Status.Equals(AttachTaskLabelServiceResponseStatus.TaskAlredyHasLabel))
                return StatusCode(StatusCodes.Status400BadRequest,
                       new AttachTaskLabelResponse
                       {
                           Message = serviceResponse.Status
                       });

            if (serviceResponse.Status.Equals(AttachTaskLabelServiceResponseStatus.InternalError))
                return StatusCode(StatusCodes.Status500InternalServerError,
                        new AttachTaskLabelResponse
                        {
                            Message = serviceResponse.Status
                        });

            return Ok(new AttachTaskLabelResponse
            {
                Message = serviceResponse.Status
            });
        }
    }
}
