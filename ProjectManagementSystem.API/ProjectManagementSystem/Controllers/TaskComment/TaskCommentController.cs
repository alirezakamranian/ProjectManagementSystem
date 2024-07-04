using Application.Services.ApiServices;
using Domain.Models.ApiModels.ProjectTask.Response;
using Domain.Models.ApiModels.TaskComment.Request;
using Domain.Models.ApiModels.TaskComment.Response;
using Domain.Models.ServiceResponses.ProjectTask;
using Domain.Models.ServiceResponses.TaskComment;
using Domain.Services.ApiServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProjectManagementSystem.Controllers.TaskComment
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskCommentController(ITaskCommentService taskCommentService) : ControllerBase
    {
        private readonly ITaskCommentService _taskCommentService = taskCommentService;

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddComment(AddTaskCommentRequest request)
        {
            var userId = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type.Equals("Id")).Value;

            var serviceResponse = await _taskCommentService
                .AddComment(request, userId);

            if (serviceResponse.Status.Equals(AddTaskCommentServiceResponseStatus.AccessDenied) ||
               serviceResponse.Status.Equals(AddTaskCommentServiceResponseStatus.TaskNotExists))
                return StatusCode(StatusCodes.Status400BadRequest,
                       new AddTaskCommentResponse
                       {
                           Message = serviceResponse.Status
                       });

            if (serviceResponse.Status.Equals(AddTaskCommentServiceResponseStatus.InternalError))
                return StatusCode(StatusCodes.Status500InternalServerError,
                        new AddTaskCommentResponse
                        {
                            Message = serviceResponse.Status
                        });

            return Ok(new AddTaskCommentResponse
            {
                Message = serviceResponse.Status
            });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetComments([FromQuery] string taskId)
        {
            var userId = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type.Equals("Id")).Value;

            var serviceResponse =await _taskCommentService
                .GetTaskComments(new() {TaskId=taskId},userId);

            if (serviceResponse.Status.Equals(GetTaskCommentServiceResponseStatus.AccessDenied) ||
               serviceResponse.Status.Equals(GetTaskCommentServiceResponseStatus.TaskNotExists))
                return StatusCode(StatusCodes.Status400BadRequest,
                       new GetTaskCommentsResponse
                       {
                           Message = serviceResponse.Status
                       });

            if (serviceResponse.Status.Equals(GetTaskCommentServiceResponseStatus.InternalError))
                return StatusCode(StatusCodes.Status500InternalServerError,
                        new GetTaskCommentsResponse
                        {
                            Message = serviceResponse.Status
                        });

            return Ok(new GetTaskCommentsResponse
            {
                Message = serviceResponse.Status,
                Comments = serviceResponse.Comments
            });
        }
    }
}
