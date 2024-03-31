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
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateProjectRequest request)
        {
            if (request == null)
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new CreateProjectResponse
                    {
                        Status = "InvaildData",
                        Message = "ProjectDetailsAreRequired!"
                    });

            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id").Value;

            var serviceResponse = await _projectService.CreateProject(request, userId);

            if (serviceResponse.Status == CreateProjectServiceResponseStatus.OrganizationNotExists)
                return StatusCode(StatusCodes.Status400BadRequest,
                    new CreateProjectResponse
                    {
                        Status = serviceResponse.Status,
                        Message = "TargetOrganizationNotExists!"
                    });

            if (serviceResponse.Status == CreateProjectServiceResponseStatus.AccessDenied)
                return StatusCode(StatusCodes.Status400BadRequest,
                    new CreateProjectResponse
                    {
                        Status = serviceResponse.Status,
                        Message = "YouDontHaveAccessForCurrentOperationInThisOrganization!"
                    });

            if (serviceResponse.Status == CreateProjectServiceResponseStatus.InternalError)
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new CreateProjectResponse
                    {
                        Status = serviceResponse.Status,
                        Message = "InternalServerError!"
                    });

            return Ok(new CreateProjectResponse
            {
                Status = serviceResponse.Status,
                Message = "ProjectCreatedSuccessfully!"
            });
        }
    }    
}
