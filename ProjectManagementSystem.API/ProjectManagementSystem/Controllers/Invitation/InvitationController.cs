using Domain.Models.Dtos.Organization.Response;
using Domain.Models.Dtos.OrganizationInvitation.Request;
using Domain.Models.Dtos.OrganizationInvitation.Response;
using Domain.Models.ServiceResponses.OrganizationInvitation;
using Domain.Services.ApiServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ProjectManagementSystem.Controllers.Invitation
{
    [Route("organization/employee/invite")]
    [ApiController]
    public class InvitationController(IOrganizationInvitationService invitationService) : ControllerBase
    {

        private readonly IOrganizationInvitationService _invitationService = invitationService;

        [Authorize]
        [HttpGet("search")]
        public async Task<IActionResult> SearchUser([FromQuery] string query)
        {
            if (query.IsNullOrEmpty() || query.Trim().Length < 3)
                return StatusCode(StatusCodes.Status400BadRequest,
                        new SearchUserResponse
                        {
                            Status = "InvalidData",
                            Message = "EnterValid(3 char)input"
                        });

            var serviceResponse = await _invitationService.SearchUser(
                new SearchUserRequst
                {
                    Query = query
                });

            if (serviceResponse.Status == SearchUserServiceResponseStatus.InternalError)
                return StatusCode(StatusCodes.Status500InternalServerError,
                       new SearchUserResponse
                       {
                           Status = serviceResponse.Status,
                           Message = "InternulServerError!"
                       });

            List<UserForResponseDto> users = [];
            foreach (var user in serviceResponse.Users)
            {
                users.Add(new()
                {
                    Email = user.Email,
                    Name = user.FullName
                });
            }

            return Ok(new SearchUserResponse
            {
                Status = serviceResponse.Status,
                Message = "results:",
                Results = users
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> InviteEmployee([FromBody] InviteEmployeeRequest request)
        {
            if (string.IsNullOrEmpty(request.UserEmail) || request == null)
                return StatusCode(StatusCodes.Status400BadRequest,
                        new InviteEmployeeResponse
                        {
                            Status = "InvaildData",
                            Message = "InviteDetailsIsRequired!"
                        });

            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id").Value;

            var serviceResponse = await _invitationService.InviteEmployee(request, userId);

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
            if (string.IsNullOrEmpty(request.InviteId) || request == null)
                return StatusCode(StatusCodes.Status400BadRequest,
                       new AcceptInvitationResponse
                       {
                           Status = "InvaildData",
                           Message = "ActionDetailsIsRequired!"
                       });

            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id").Value;

            var serviceResponse = await _invitationService.AcceptOrganizationInvitation(request, userId);

            if (serviceResponse.Status == AcceptOrganizationInvitationServiceResponseStatus.NotificationNotExists)
                return StatusCode(StatusCodes.Status400BadRequest,
                       new AcceptInvitationResponse
                       {
                           Status = serviceResponse.Status,
                           Message = "TargetNotificationNotExists!!"
                       });

            if (serviceResponse.Status == AcceptOrganizationInvitationServiceResponseStatus.InternalError)
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
            if (string.IsNullOrEmpty(request.InviteId) || request == null)
                return StatusCode(StatusCodes.Status400BadRequest,
                       new RejectInvitationRespons
                       {
                           Status = "InvaildData",
                           Message = "ActionDetailsIsRequired!"
                       });

            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id").Value;

            var serviceResponse = await _invitationService.RejectOrganizationInvitation(request, userId);

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
