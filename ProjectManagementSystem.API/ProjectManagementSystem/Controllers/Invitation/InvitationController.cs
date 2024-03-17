using Domain.Models.Dtos.Organization.Response;
using Domain.Models.Dtos.OrganizationInvitation.Request;
using Domain.Models.Dtos.OrganizationInvitation.Response;
using Domain.Models.ServiceResponses.OrganizationInvitation;
using Domain.Services.ApiServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProjectManagementSystem.Controllers.Invitation
{
    [Route("organization/invite")]
    [ApiController]
    public class InvitationController(IOrganizationInvitationService invitationService) : ControllerBase
    {

        private readonly IOrganizationInvitationService _invitationService=invitationService;

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> InviteEmployee([FromBody] InviteEmployeeRequest request)
        {
            if (request == null)
                return StatusCode(StatusCodes.Status400BadRequest,
                        new InviteEmployeeResponse
                        {
                            Status = "InvaildData",
                            Message = "InviteDetailsIsRequired!"
                        });

            var email = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Email").Value;

            var serviceResponse = await _invitationService.InviteEmployee(request, email);

            if (serviceResponse.Status == InviteEmployeeServiceResponseStatus.InternalError)
                return StatusCode(StatusCodes.Status500InternalServerError,
                        new InviteEmployeeResponse
                        {
                            Status = serviceResponse.Status,
                            Message = "InternulServerError!"
                        });

            if (serviceResponse.Status == InviteEmployeeServiceResponseStatus.UserNotExists)
                return StatusCode(StatusCodes.Status400BadRequest,
                        new InviteEmployeeResponse
                        {
                            Status = serviceResponse.Status,
                            Message = "UserWithThisEmailNoteExists!"
                        });

            return Ok(new GetSubscribedOrganizationsResponse
            {
                Status = serviceResponse.Status,
                Message = "RequestSent!",
            });
        }

        [Authorize]
        [HttpPost("accept")]
        public async Task<IActionResult> Accept([FromBody] AcceptInvitationRequest request)
        {
            if (request == null)
                return StatusCode(StatusCodes.Status400BadRequest,
                       new AcceptInvitationResponse
                       {
                           Status = "InvaildData",
                           Message = "ActionDetailsIsRequired!"
                       });

            var email = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Email").Value;

            var serviceResponse = await _invitationService.AcceptOrganizationInvitation(request, email);

            if (serviceResponse.Status == AcceptOrganizationInvitationServiceResponseStatus.NotificationNotExists)
                return StatusCode(StatusCodes.Status400BadRequest,
                       new AcceptInvitationResponse
                       {
                           Status = serviceResponse.Status,
                           Message = "TargetNotificationNotExists!!"
                       });

            if (serviceResponse.Status==AcceptOrganizationInvitationServiceResponseStatus.InternalError)
                return StatusCode(StatusCodes.Status500InternalServerError,
                       new AcceptInvitationResponse
                       {
                           Status = serviceResponse.Status,
                           Message = "InternulServerError!"
                       });
           
            return Ok(new AcceptInvitationResponse
            {
                Status = serviceResponse.Status,
                Message = "InviteAcceptedSuccessfully!"
            });
        }

        [Authorize]
        [HttpPost("reject")]
        public async Task<IActionResult> Reject([FromBody] RejectInvitationRequest request)
        {
            if (request == null)
                return StatusCode(StatusCodes.Status400BadRequest,
                       new RejectInvitationRespons
                       {
                           Status = "InvaildData",
                           Message = "ActionDetailsIsRequired!"
                       });

            var email = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Email").Value;

            var serviceResponse = await _invitationService.RejectOrganizationInvitation(request, email);

            if (serviceResponse.Status == RejectOrganizationInvitationServiceResponseStatus.NotificationNotExists)
                return StatusCode(StatusCodes.Status400BadRequest,
                       new RejectInvitationRespons
                       {
                           Status = serviceResponse.Status,
                           Message = "TargetNotificationNotExists!!"
                       });

            if (serviceResponse.Status == RejectOrganizationInvitationServiceResponseStatus.InternalError)
                return StatusCode(StatusCodes.Status500InternalServerError,
                       new RejectInvitationRespons
                       {
                           Status = serviceResponse.Status,
                           Message = "InternulServerError!"
                       });

            return Ok(new RejectInvitationRespons
            {
                Status = serviceResponse.Status,
                Message = "InviteRejectedSuccessfully!"
            });
        }
    }
}
