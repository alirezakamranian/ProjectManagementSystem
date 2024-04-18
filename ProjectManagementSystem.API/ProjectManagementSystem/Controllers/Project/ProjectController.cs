using Domain.Models.Dtos.Auth.Response;
using Domain.Models.Dtos.Project.Request;
using Domain.Models.Dtos.Project.Response;
using Domain.Models.ServiceResponses.Project;
using Domain.Services.ApiServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace ProjectManagementSystem.Controllers.Project
{
    [Route("organization/project")]
    [ApiController]
    public class ProjectController(IProjectService projectService) : ControllerBase
    {
        private readonly IProjectService _projectService = projectService;

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string id)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("Id")).Value;

            var serviceResponse = await _projectService
                .GetProject(new() { ProjectId = id }, userId);

            if (serviceResponse.Status.Equals(GetProjectServiceResponseStatus.ProjectNotExists) ||
                serviceResponse.Status.Equals(GetProjectServiceResponseStatus.AccessDenied))
                return StatusCode(StatusCodes.Status400BadRequest,
                    new GetProjectResponse
                    {
                        Message = serviceResponse.Status
                    });

            if (serviceResponse.Status.Equals(GetProjectServiceResponseStatus.InternalError))
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new GetProjectResponse
                    {
                        Message = serviceResponse.Status
                    });

            var taskLists = new List<ProjectTaskListForResponseDto>();

            foreach (var tl in serviceResponse.Project.ProjectTaskLists)
            {
                var tasks = new List<ProjectTaskForResponseDto>();

                foreach (var t in tl.ProjectTasks)
                {
                    tasks.Add(new()
                    {
                        Id = t.Id.ToString(),
                        Title = t.Title,
                        Description = t.Description,
                        Priority = t.Priority
                    });
                }

                taskLists.Add(new()
                {
                    Id = tl.Id.ToString(),
                    Name = tl.Name,
                    Priority = tl.Priority,
                    Tasks = tasks
                });
            }

            return Ok(new GetProjectResponse
            {
                Message = serviceResponse.Status,
                Project = new()
                {
                    Id = serviceResponse.Project.Id.ToString(),
                    Name = serviceResponse.Project.Name,
                    Description = serviceResponse.Project.Description,
                    StartDate = serviceResponse.Project.StartDate,
                    DeadLine = serviceResponse.Project.DeadLine,
                    EndDate = serviceResponse.Project.EndDate,
                    Status = serviceResponse.Status,
                    ProjectTaskLists = taskLists
                }
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProjectRequest request)
        {
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new CreateProjectResponse
                    {
                        Message = "ProjectDetailsAreRequired!"
                    });

            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("Id")).Value;

            var serviceResponse = await _projectService.CreateProject(request, userId);

            if (serviceResponse.Status.Equals(CreateProjectServiceResponseStatus.OrganizationNotExists) ||
                serviceResponse.Status.Equals(CreateProjectServiceResponseStatus.AccessDenied))
                return StatusCode(StatusCodes.Status400BadRequest,
                    new CreateProjectResponse
                    {
                        Message = serviceResponse.Status
                    });

            if (serviceResponse.Status.Equals(CreateProjectServiceResponseStatus.InternalError))
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new CreateProjectResponse
                    {
                        Message = serviceResponse.Status
                    });

            return Ok(new CreateProjectResponse
            {
                Message = serviceResponse.Status
            });
        }
    }
}
