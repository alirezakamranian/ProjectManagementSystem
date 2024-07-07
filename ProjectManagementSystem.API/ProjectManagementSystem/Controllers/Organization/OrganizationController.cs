using Azure.Core;
using Domain.Models.ApiModels.Organization.Request;
using Domain.Models.ApiModels.Auth.Response;
using Domain.Models.ApiModels.Organization.Response;
using Domain.Models.ServiceResponses.Organization;
using Domain.Services.ApiServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Domain.Models.Dtos.Organization;
using Domain.Models.Dtos.Project;

namespace ProjectManagementSystem.Controllers.Organization
{
    /// <summary>
    /// Provides OrganizationService using REST-ful services
    /// </summary>
    /// <param name="organizationService"></param>
    [Route("organization")]
    [ApiController]
    public class OrganizationController(IOrganizationService organizationService) : ControllerBase
    {

        private readonly IOrganizationService _organizationService = organizationService;

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateOrganization([FromBody] CreateOrganizationRequest request)
        {
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest,
                        new CreateOrganizationResponse
                        {
                            Message = "OrganizationDetailIsInvalid"
                        });

            var userId = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type.Equals("Id")).Value;

            var serviceResponse = await _organizationService
                .CreateOrganization(request, userId);

            if (serviceResponse.Status.Equals(CreateOrganizationServiceResponseStatus.InternalError))
                return StatusCode(StatusCodes.Status500InternalServerError,
                     new RefreshTokenResponse
                     {
                         Message = serviceResponse.Status
                     });

            return Ok(new CreateOrganizationResponse
            {
                Message = serviceResponse.Status,
                Id = serviceResponse.OrgId
            });
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateOrganization([FromBody] UpdateOrganizationRequest request)
        {
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest,
                        new UpdateOrganizationResponse
                        {
                            Message = "OrganizationDetailsIsRequired"
                        });

            var userId = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type.Equals("Id")).Value;

            var serviceResponse = await _organizationService
                .UpdateOrganization(request, userId);

            if (serviceResponse.Status.Equals(UpdateOrganizationServiceResponseStatus.OrganizationNotExists) ||
                serviceResponse.Status.Equals(UpdateOrganizationServiceResponseStatus.AccessDenied))
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

            var userId = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type.Equals("Id")).Value;

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

            return Ok(new GetOrganizationResponse
            {
                Message = serviceResponse.Status,
                Name = serviceResponse.Name,
                Description = serviceResponse.Description,
                AvatarUrl = serviceResponse.AvatarUrl,
                Projects = serviceResponse.Projects,
                Employees = serviceResponse.Employees
            });
        }

        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetSubscribedOrganizations()
        {
            var userId = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type.Equals("Id")).Value;

            var serviceResponse = await _organizationService
                .GetSubscribedOrganizations(userId);

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

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Remove([FromQuery] string organizationId)
        {
            var userId = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type.Equals("Id")).Value;

            var serviceResponse = await _organizationService
                .RemoveOrganization(new()
                {
                    OrganizationId = organizationId
                }, userId);

            if (serviceResponse.Status.Equals(RemoveOrganizationServiceResponseStatus.OrganizationNotExists) ||
                serviceResponse.Status.Equals(RemoveOrganizationServiceResponseStatus.AccessDenied))
                return StatusCode(StatusCodes.Status400BadRequest,
                        new RemoveOrganiztionResponse
                        {
                            Message = serviceResponse.Status
                        });

            if (serviceResponse.Status.Equals(RemoveOrganizationServiceResponseStatus.InternalError))
                return StatusCode(StatusCodes.Status500InternalServerError,
                        new RemoveOrganiztionResponse
                        {
                            Message = serviceResponse.Status
                        });

            return Ok(new RemoveOrganiztionResponse
            {
                Message = serviceResponse.Status
            });
        }
    }
}
