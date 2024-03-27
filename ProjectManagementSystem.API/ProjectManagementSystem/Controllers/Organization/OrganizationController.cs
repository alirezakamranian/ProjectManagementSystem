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

            var email = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Email").Value;

            var serviceResponse = await _organizationService.CreateOrganization(request, email);

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

            var email = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Email").Value;

            var serviceResponse = await _organizationService.UpdateOrganization(request, email);

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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrganization(int id)
        {

            var email = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Email").Value;

            var serviceResponse = await _organizationService
                .GetOrganization(new GetOrganizationRequest 
                {
                    OrganizationId=id,
                    Email=email
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

            return Ok(new GetOrganizationResponse
            {
                Status = serviceResponse.Status,
                Message = "Success!",
                Name = serviceResponse.Name,
                Projects = serviceResponse.Projects
            });
        }

        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetSubscribedOrganizations()
        {
            var email = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Email").Value;

            var serviceResponse = await _organizationService.GetSubscribedOrganizations(email);

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
