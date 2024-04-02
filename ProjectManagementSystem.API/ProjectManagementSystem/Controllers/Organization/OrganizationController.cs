using Azure.Core;
using Domain.Models.Dtos.Auth.Response;
using Domain.Models.Dtos.Organization.Request;
using Domain.Models.Dtos.Organization.Response;
using Domain.Models.ServiceResponses.Organization;
using Domain.Services.ApiServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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
            if (request.Equals(null))
                return StatusCode(StatusCodes.Status400BadRequest,
                        new CreateOrganizationResponse
                        {
                            Message = "OrganizationDetailIsRequired!"
                        });

            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("Id")).Value;

            var serviceResponse = await _organizationService.CreateOrganization(request, userId);

            if (serviceResponse.Status.Equals(CreateOrganizationServiceResponseStatus.InternalError))
                return StatusCode(StatusCodes.Status500InternalServerError,
                     new RefreshTokenResponse
                     {
                         Message = serviceResponse.Status
                     });

            return Ok(new CreateOrganizationResponse
            {
                Message = serviceResponse.Status
            });
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateOrganization([FromBody] UpdateOrganizationRequest request)
        {
            if (request.Equals(null))
                return StatusCode(StatusCodes.Status400BadRequest,
                        new UpdateOrganizationResponse
                        {
                            Message = "OrganizationDetailsIsRequired"
                        });

            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("Id")).Value;

            var serviceResponse = await _organizationService.UpdateOrganization(request, userId);

            if (serviceResponse.Status.Equals(UpdateOrganizationServiceResponseStatus.OrganizationNotExists))
                return StatusCode(StatusCodes.Status400BadRequest,
                        new UpdateOrganizationResponse
                        {
                            Message = serviceResponse.Status
                        });

            if (serviceResponse.Status.Equals(UpdateOrganizationServiceResponseStatus.InternalError))
                return StatusCode(StatusCodes.Status500InternalServerError,
                        new UpdateOrganizationResponse
                        {
                            Message = serviceResponse.Status
                        });

            return Ok(new UpdateOrganizationResponse
            {
                Message = serviceResponse.Status
            });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetOrganization([FromQuery] string id)
        {
            if (id.IsNullOrEmpty())
                return StatusCode(StatusCodes.Status400BadRequest,
                        new GetOrganizationResponse
                        {
                            Message = "OrganizationDetailsIsRequired"
                        });

            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("Id")).Value;

            var serviceResponse = await _organizationService
                .GetOrganization(new GetOrganizationRequest
                {
                    OrganizationId = id,
                    UserId = userId
                });

            if (serviceResponse.Status.Equals(GetOrganizationServiceResponseStatus.OrganizationNotExists) ||
                serviceResponse.Status.Equals(GetOrganizationServiceResponseStatus.AccessDenied))
                return StatusCode(StatusCodes.Status400BadRequest,
                        new GetOrganizationResponse
                        {
                            Message = serviceResponse.Status
                        });

            if (serviceResponse.Status.Equals(GetOrganizationServiceResponseStatus.InternalError))
                return StatusCode(StatusCodes.Status500InternalServerError,
                        new GetOrganizationResponse
                        {
                            Message = serviceResponse.Status
                        });

            List<ProjectForResponseDto> projects = [];

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
                Message = serviceResponse.Status,
                Name = serviceResponse.Name,
                Projects = projects
            });
        }

        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetSubscribedOrganizations()
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("Id")).Value;

            var serviceResponse = await _organizationService.GetSubscribedOrganizations(userId);

            if (serviceResponse.Status.Equals(GetSubscribedOrganizationsServiceResponseStatus.InternalError))
                return StatusCode(StatusCodes.Status500InternalServerError,
                        new GetSubscribedOrganizationsResponse
                        {
                            Message = serviceResponse.Status
                        });

            return Ok(new GetSubscribedOrganizationsResponse
            {
                Message = serviceResponse.Status,
                Organizations = serviceResponse.Organizations
            });

        }
    }
}
