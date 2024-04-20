using Domain.Models.Dtos.OrganizationEmployee.response;
using Domain.Models.Dtos.ProjectTaskList.Request;
using Domain.Models.Dtos.ProjectTaskList.Response;
using Domain.Models.ServiceResponses.OrganizationEmployee;
using Domain.Services.ApiServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain.Models.ServiceResponses.ProjectTaskList;
using Microsoft.AspNetCore.Authorization;
namespace ProjectManagementSystem.Controllers.ProjectTaskList
{
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
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest,
                       new CreateTaskListResponse
                       {
                           Message = "DetailsAreRequired!"
                       });

            var serviceResponse = await _taskListService.CreateTaskList(request);

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
                Message = serviceResponse.Status
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
    }
}
