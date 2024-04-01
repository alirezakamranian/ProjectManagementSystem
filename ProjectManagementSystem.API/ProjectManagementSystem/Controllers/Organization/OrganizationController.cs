using Domain.Models.Dtos.Auth.Response;
using Domain.Models.Dtos.Organization.Request;
using Domain.Models.Dtos.Organization.Response;
using Domain.Models.ServiceResponses.Organization;
using Domain.Services.ApiServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProjectManagementSystem.Controllers.Organization
{
    [Route("organization")]
    [ApiController]
    public class OrganizationController(IOrganizationService organizationService) : ControllerBase
    {

        private readonly IOrganizationService _organizationService = organizationService;

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateOrganization([FromBody] CreateOrganizationRequest request)
        {
            if (request == null)
                return StatusCode(StatusCodes.Status400BadRequest,
                        new CreateOrganizationResponse
                        {
                            Status = "InvaildData",
                            Message = "OrganizationNameIsRequired!"
                        });

            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id").Value;

            var serviceResponse = await _organizationService.CreateOrganization(request, userId);

            if (serviceResponse.Status == CreateOrganizationServiceResponseStatus.InternalError)
                return StatusCode(StatusCodes.Status500InternalServerError,
                     new RefreshTokenResponse
                     {
                         Status = serviceResponse.Status,
                         Message = "InternulServerError!"
                     });

            return Ok(new CreateOrganizationResponse
            {
                Status = serviceResponse.Status,
                Message = "OrganizationCreatedSuccessfully!"
            });
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateOrganization([FromBody] UpdateOrganizationRequest request)
        {
            if (request == null)
                return StatusCode(StatusCodes.Status400BadRequest,
                        new UpdateOrganizationResponse
                        {
                            Status = "InvaildData",
                            Message = "OrganizationDetailsIsRequired"
                        });

            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id").Value;

            var serviceResponse = await _organizationService.UpdateOrganization(request, userId);

            if (serviceResponse.Status == UpdateOrganizationServiceResponseStatus.OrganizationNotExists)
                return StatusCode(StatusCodes.Status400BadRequest,
                        new CreateOrganizationResponse
                        {
                            Status = serviceResponse.Status,
                            Message = "OrganizationWithThisUserOrIdNotExists!"
                        });

            if (serviceResponse.Status == UpdateOrganizationServiceResponseStatus.InternalError)
                return StatusCode(StatusCodes.Status500InternalServerError,
                        new CreateOrganizationResponse
                        {
                            Status = serviceResponse.Status,
                            Message = "InternulServerError!"
                        });

            return Ok(new CreateOrganizationResponse
            {
                Status = serviceResponse.Status,
                Message = "OrganizationUpdatedSuccessfully!"
            });
        }

        [Authorize]
        [HttpGet("get")]
        public async Task<IActionResult> GetOrganization([FromQuery] string id)
        {

            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id").Value;

            var serviceResponse = await _organizationService
                .GetOrganization(new GetOrganizationRequest
                {
                    OrganizationId = id,
                    UserId = userId
                });

            if (serviceResponse.Status == GetOrganizationServiceResponseStatus.OrganizationNotExists)
                return StatusCode(StatusCodes.Status400BadRequest,
                        new GetOrganizationResponse
                        {
                            Status = serviceResponse.Status,
                            Message = "OrganizationWithThisUserOrIdNotExists!"
                        });

            if (serviceResponse.Status == GetOrganizationServiceResponseStatus.InternalError)
                return StatusCode(StatusCodes.Status500InternalServerError,
                        new GetOrganizationResponse
                        {
                            Status = serviceResponse.Status,
                            Message = "InternulServerError!"
                        });

            if (serviceResponse.Status == GetOrganizationServiceResponseStatus.AccessDenied)
                return StatusCode(StatusCodes.Status400BadRequest,
                        new GetOrganizationResponse
                        {
                            Status = serviceResponse.Status,
                            Message = "YouAreNotAnEmployeeOfThisOrganization!"
                        });

            List<ProjectForResponseDto> projects = new();

            foreach (var p in serviceResponse.Projects)
            {
                projects.Add(new()
                {
                    Id = p.Id.ToString(),
                    Name = p.Name,
                    Description = p.Description,
                    Status = p.Status,
                    StartDate = p.StartDate,
                    DeadLine = p.DeadLine,
                    EndDate = p.EndDate,
                });
            }

            return Ok(new GetOrganizationResponse
            {
                Status = serviceResponse.Status,
                Message = "Success!",
                Name = serviceResponse.Name,
                Projects = projects
            });
        }

        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetSubscribedOrganizations()
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id").Value;

            var serviceResponse = await _organizationService.GetSubscribedOrganizations(userId);

            if (serviceResponse.Status == GetSubscribedOrganizationsServiceResponseStatus.InternalError)
                return StatusCode(StatusCodes.Status500InternalServerError,
                        new GetSubscribedOrganizationsResponse
                        {
                            Status = serviceResponse.Status,
                            Message = "InternulServerError!"
                        });

            return Ok(new GetSubscribedOrganizationsResponse
            {
                Status = serviceResponse.Status,
                Message = "Success!",
                Organizations = serviceResponse.Organizations
            });

        }
    }
}
