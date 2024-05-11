using Domain.Models.ApiModels.Project.Request;
using Domain.Models.ApiModels.Project.Response;
using Domain.Models.ApiModels.Auth.Response;
using Domain.Models.ServiceResponses.Project;
using Domain.Services.ApiServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using Domain.Models.Dtos.Project;
using Domain.Services.InternalServices;

namespace ProjectManagementSystem.Controllers.Project
{
    /// <summary>
    /// Provides ProjectService using REST-ful services
    /// </summary>
    /// <param name="projectService"></param>
    /// <param name="storageService"></param>
    [Route("organization/project")]
    [ApiController]
    public class ProjectController(IProjectService projectService) : ControllerBase
    {
        private readonly IProjectService _projectService = projectService;

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string id)
        {
            var userId = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type.Equals("Id")).Value;

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

            return Ok(new GetProjectResponse
            {
                Message = serviceResponse.Status,
                Project = serviceResponse.Project
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

            var userId = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type.Equals("Id")).Value;

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
                Message = serviceResponse.Status,
                Id = serviceResponse.ProjectId
            });
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] string id)
        {
            var userId = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type.Equals("Id")).Value;

            var serviceResponse = await _projectService
                .DeleteProject(new() { ProjectId = id }, userId);

            if (serviceResponse.Status.Equals(DeleteProjectServiceResponseStatus.ProjectNotExists) ||
               serviceResponse.Status.Equals(DeleteProjectServiceResponseStatus.AccessDenied))
                return StatusCode(StatusCodes.Status400BadRequest,
                    new DeleteProjectResponse
                    {
                        Message = serviceResponse.Status
                    });

            if (serviceResponse.Status.Equals(DeleteProjectServiceResponseStatus.InternalError))
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new DeleteProjectResponse
                    {
                        Message = serviceResponse.Status
                    });

            return Ok(new DeleteProjectResponse
            {
                Message = serviceResponse.Status
            });
        }

        [Authorize]
        [HttpPatch]
        public async Task<IActionResult> ChangeLeader(ChangeProjectLeaderRequest request)
        {
            var userId = HttpContext.User.Claims
               .FirstOrDefault(c => c.Type.Equals("Id")).Value;

            var serviceResponse = await _projectService
                .ChangeLeadr(request, userId);

            if (serviceResponse.Status.Equals(ChangeProjectLeaderServiceResponseStatus.ProjectNotExists) ||
               serviceResponse.Status.Equals(ChangeProjectLeaderServiceResponseStatus.AccessDenied) ||
               serviceResponse.Status.Equals(ChangeProjectLeaderServiceResponseStatus.LeaderNotExists))
                return StatusCode(StatusCodes.Status400BadRequest,
                    new ChangeProjectLeaderResponse
                    {
                        Message = serviceResponse.Status
                    });

            if (serviceResponse.Status.Equals(ChangeProjectLeaderServiceResponseStatus.InternalError))
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ChangeProjectLeaderResponse
                    {
                        Message = serviceResponse.Status
                    });

            return Ok(new ChangeProjectLeaderResponse
            {
                Message = serviceResponse.Status
            });
        }
    }
}
