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
                            Message = "OrganizationNameIsRequired"
                        });
            var email = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Email").Value;

            var serviceResponse= await  _organizationService.CreateOrganization(request, email);

            if (serviceResponse.Status == CreateOrganizationServiceResponseStatus.InternalError) 
                return StatusCode(StatusCodes.Status500InternalServerError,
                     new RefreshTokenResponse
                     {
                         Status = serviceResponse.Status,
                         Message = "InternulServerError!"
                     });

            return Ok(new CreateOrganizationResponse
            {
                Status= serviceResponse.Status,
                Message= "OrganizationCreatedSuccessfully!"
            });
        }
    }
}
