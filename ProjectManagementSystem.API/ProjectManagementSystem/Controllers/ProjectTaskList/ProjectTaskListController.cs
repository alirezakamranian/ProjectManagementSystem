using Domain.Models.ApiModels.OrganizationEmployee.response;
using Domain.Models.ApiModels.ProjectTaskList.Response;
using Domain.Models.ServiceResponses.OrganizationEmployee;
using Domain.Services.ApiServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain.Models.ServiceResponses.ProjectTaskList;
using Microsoft.AspNetCore.Authorization;
using Domain.Models.ApiModels.ProjectTask.Response;
using Domain.Models.ApiModels.ProjectTaskList.Request;
namespace ProjectManagementSystem.Controllers.ProjectTaskList
{
    /// <summary>
    /// Provides ProjectTaskListService using REST-ful services
    /// </summary>
    /// <param name="taskListService"></param>
    [Route("organization/project/tasklist")]
    [ApiController]
    public class ProjectTaskListController(
    IProjectTaskListService taskListService) : ControllerBase
    {
        private readonly IProjectTaskListService _taskListService = taskListService;

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateTaskList(CreateTaskListRequest request)
        {
            var userId = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type.Equals("Id")).Value;

            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest,
                       new CreateTaskListResponse
                       {
                           Message = "DetailsAreRequired!"
                       });

            var serviceResponse = await _taskListService
                .CreateTaskList(request, userId);

            if (serviceResponse.Status.Equals(ProjectTaskListServiceResponseStatus.ProjectNotExists))
                return StatusCode(StatusCodes.Status400BadRequest,
                     new CreateTaskListResponse
                     {
                         Message = serviceResponse.Status
                     });

            if (serviceResponse.Status.Equals(ProjectTaskListServiceResponseStatus.InternalError))
                return StatusCode(StatusCodes.Status500InternalServerError,
                     new CreateTaskListResponse
                     {
                         Message = serviceResponse.Status
                     });

            return Ok(new CreateTaskListResponse
            {
                Message = serviceResponse.Status,
                TaskList = serviceResponse.TaskList
            });
        }

        [Authorize]
        [HttpPatch]
        public async Task<IActionResult> ChangePriority(ChangeTaskListPriorityRequest request)
        {
            var userId = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type.Equals("Id")).Value;

            var serviceResponse = await _taskListService
                .ChangeTaskListPriority(request, userId);

            if (serviceResponse.Status.Equals(ChangeTaskListPriorityServiceResponseStatus.AccessDenied) ||
                serviceResponse.Status.Equals(ChangeTaskListPriorityServiceResponseStatus.InvalidPriority) ||
                serviceResponse.Status.Equals(ChangeTaskListPriorityServiceResponseStatus.TaskListNotExists))
                return StatusCode(StatusCodes.Status400BadRequest,
                     new ChangeTaskListPriorityResponse
                     {
                         Message = serviceResponse.Status
                     });

            if (serviceResponse.Status.Equals(ProjectTaskListServiceResponseStatus.InternalError))
                return StatusCode(StatusCodes.Status500InternalServerError,
                     new ChangeTaskListPriorityResponse
                     {
                         Message = serviceResponse.Status
                     });

            return Ok(new ChangeTaskListPriorityResponse
            {
                Message = serviceResponse.Status
            });
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] string id)
        {
            var userId = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type.Equals("Id")).Value;

            var serviceResponse = await _taskListService
                .DeleteTaskList(new() { TaskListId = id }, userId);

            if (serviceResponse.Status.Equals(DeleteTaskListServiceResponseStatus.AccessDenied) ||
               serviceResponse.Status.Equals(DeleteTaskListServiceResponseStatus.TaskListNotExists))
                return StatusCode(StatusCodes.Status400BadRequest,
                     new DeleteTaskListResponse
                     {
                         Message = serviceResponse.Status
                     });

            if (serviceResponse.Status.Equals(DeleteTaskListServiceResponseStatus.InternalError))
                return StatusCode(StatusCodes.Status500InternalServerError,
                     new DeleteTaskListResponse
                     {
                         Message = serviceResponse.Status
                     });

            return Ok(new DeleteTaskListResponse
            {
                Message = serviceResponse.Status
            });
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateTaskListRequest request)
        {
            var userId = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type.Equals("Id")).Value;

            var serviceResponse = await _taskListService
                .UpdateTaskList(request, userId);

            if (serviceResponse.Status.Equals(UpdateTaskListServiceResponseStatus.AccessDenied) ||
              serviceResponse.Status.Equals(UpdateTaskListServiceResponseStatus.TaskListNotExists))
                return StatusCode(StatusCodes.Status400BadRequest,
                     new UpdateTaskListResponse
                     {
                         Message = serviceResponse.Status
                     });

            if (serviceResponse.Status.Equals(UpdateTaskListServiceResponseStatus.InternalError))
                return StatusCode(StatusCodes.Status500InternalServerError,
                     new UpdateTaskListResponse
                     {
                         Message = serviceResponse.Status
                     });

            return Ok(new UpdateTaskListResponse
            {
                Message = serviceResponse.Status
            });

        }
    }
}
