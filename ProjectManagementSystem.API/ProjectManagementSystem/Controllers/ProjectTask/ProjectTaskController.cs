using Domain.Models.ApiModels.ProjectTask.Request;
using Domain.Models.ApiModels.ProjectMember.Response;
using Domain.Models.ApiModels.ProjectTask.Response;
using Domain.Models.ServiceResponses.ProjectMember;
using Domain.Models.ServiceResponses.ProjectTask;
using Domain.Services.ApiServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProjectManagementSystem.Controllers.ProjectTask
{
    /// <summary>
    /// Provides ProjectTaskService using REST-ful services
    /// </summary>
    /// <param name="taskService"></param>
    [Route("/organization/project/tasklist/task")]
    [ApiController]
    public class ProjectTaskController(IProjectTaskService taskService) : ControllerBase
    {
        private readonly IProjectTaskService _taskService = taskService;

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string Id)
        {
            var userId = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type.Equals("Id")).Value;

            var serviceResponse = await _taskService
                .GetTask(new() { TaskId = Id }, userId);

            if (serviceResponse.Status.Equals(GetProjectTaskServiceResponseStatus.AccessDenied) ||
                serviceResponse.Status.Equals(GetProjectTaskServiceResponseStatus.TaskNotExists))
                return StatusCode(StatusCodes.Status400BadRequest,
                       new GetProjectTaskResponse
                       {
                           Message = serviceResponse.Status
                       });

            if (serviceResponse.Status.Equals(GetProjectTaskServiceResponseStatus.InternalError))
                return StatusCode(StatusCodes.Status500InternalServerError,
                        new GetProjectTaskResponse
                        {
                            Message = serviceResponse.Status
                        });

            return Ok(new GetProjectTaskResponse
            {
                Message = serviceResponse.Status,
                Task = new()
                {
                    Id = serviceResponse.Task.Id.ToString(),
                    Title = serviceResponse.Task.Title,
                    Description = serviceResponse.Task.Description,
                    Priority = serviceResponse.Task.Priority
                }
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateProjectTaskRequest request)
        {
            var userId = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type.Equals("Id")).Value;

            var serviceResponse = await _taskService
                .CreateTask(request, userId);

            if (serviceResponse.Status.Equals(CreateProjectTaskServiceResponseStatus.AccessDenied) ||
                serviceResponse.Status.Equals(CreateProjectTaskServiceResponseStatus.TaskListNotExists))
                return StatusCode(StatusCodes.Status400BadRequest,
                        new CreateProjectTaskResponse
                        {
                            Message = serviceResponse.Status
                        });

            if (serviceResponse.Status.Equals(CreateProjectTaskServiceResponseStatus.InternalError))
                return StatusCode(StatusCodes.Status500InternalServerError,
                        new CreateProjectTaskResponse
                        {
                            Message = serviceResponse.Status
                        });

            return Ok(new CreateProjectTaskResponse
            {
                Message = serviceResponse.Status,
                NewTask = serviceResponse.NewTask
            });

        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] string Id)
        {
            var userId = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type.Equals("Id")).Value;

            var serviceResponse = await _taskService
                .DeleteTask(new() { TaskId = Id }, userId);

            if (serviceResponse.Status.Equals(DeleteProjectTaskServiceResponseStatus.AccessDenied) ||
               serviceResponse.Status.Equals(DeleteProjectTaskServiceResponseStatus.TaskNotExists))
                return StatusCode(StatusCodes.Status400BadRequest,
                        new DeleteProjectTaskResponse
                        {
                            Message = serviceResponse.Status
                        });

            if (serviceResponse.Status.Equals(DeleteProjectTaskServiceResponseStatus.InternalError))
                return StatusCode(StatusCodes.Status500InternalServerError,
                        new DeleteProjectTaskResponse
                        {
                            Message = serviceResponse.Status
                        });

            return Ok(new DeleteProjectTaskResponse
            {
                Message = serviceResponse.Status
            });
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateProjectTaskRequest request)
        {
            var userId = HttpContext.User.Claims
               .FirstOrDefault(c => c.Type.Equals("Id")).Value;

            var serviceResponse = await _taskService
                .UpdateTask(request, userId);

            if (serviceResponse.Status.Equals(UpdateProjectTaskServiceResponseStatus.AccessDenied) ||
               serviceResponse.Status.Equals(UpdateProjectTaskServiceResponseStatus.TaskNotExists))
                return StatusCode(StatusCodes.Status400BadRequest,
                        new UpdateProjectTaskResponse
                        {
                            Message = serviceResponse.Status
                        });

            if (serviceResponse.Status.Equals(UpdateProjectTaskServiceResponseStatus.InternalError))
                return StatusCode(StatusCodes.Status500InternalServerError,
                        new UpdateProjectTaskResponse
                        {
                            Message = serviceResponse.Status
                        });

            return Ok(new UpdateProjectTaskResponse
            {
                Message = serviceResponse.Status
            });
        }

        [Authorize]
        [HttpPatch]
        public async Task<IActionResult> ChangePriority(ChangeProjectTaskPriorityRequest request)
        {
            var userId = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type.Equals("Id")).Value;

            var serviceResponse = await _taskService
                .ChangePriority(request, userId);

            if (serviceResponse.Status.Equals(ChangeProjectTaskPriorityServiceResponseStatus.AccessDenied) ||
             serviceResponse.Status.Equals(ChangeProjectTaskPriorityServiceResponseStatus.TaskNotExists))
                return StatusCode(StatusCodes.Status400BadRequest,
                        new ChangeProjectTaskPriorityResponse
                        {
                            Message = serviceResponse.Status
                        });

            if (serviceResponse.Status.Equals(ChangeProjectTaskPriorityServiceResponseStatus.InternalError))
                return StatusCode(StatusCodes.Status500InternalServerError,
                        new ChangeProjectTaskPriorityResponse
                        {
                            Message = serviceResponse.Status
                        });

            return Ok(new ChangeProjectTaskPriorityResponse
            {
                Message = serviceResponse.Status
            });
        }

        [Authorize]
        [HttpPatch("changetasklist")]
        public async Task<IActionResult> ChangeTaskList(ChangeProjectTasksTaskListRequest request)
        {
            var userId = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type.Equals("Id")).Value;

            var serviceResponse = await _taskService
                .ChangeTaskList(request, userId);

            if (serviceResponse.Status.Equals(ChangeProjectTasksTaskListServiceResponseStatus.AccessDenied) ||
             serviceResponse.Status.Equals(ChangeProjectTasksTaskListServiceResponseStatus.TaskNotExists)||
             serviceResponse.Status.Equals(ChangeProjectTasksTaskListServiceResponseStatus.TaskListNotExists))
                return StatusCode(StatusCodes.Status400BadRequest,
                        new ChangeProjectTasksTaskListResponse
                        {
                            Message = serviceResponse.Status
                        });

            if (serviceResponse.Status.Equals(ChangeProjectTasksTaskListServiceResponseStatus.InternalError))
                return StatusCode(StatusCodes.Status500InternalServerError,
                        new ChangeProjectTasksTaskListResponse
                        {
                            Message = serviceResponse.Status
                        });

            return Ok(new ChangeProjectTaskPriorityResponse
            {
                Message = serviceResponse.Status
            });
        }
    }
}
