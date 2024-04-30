using Domain.Models.ApiModels.OrganizationInvitation.Request;
using Domain.Models.ApiModels.OrganizationInvitation.Response;
using Domain.Models.Dtos.User;
using Domain.Models.ServiceResponses.OrganizationInvitation;
using Domain.Services.ApiServices;
using Microsoft.AspNetCore.Authorization;
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
                            Message = "EnterValid(3 char)input"
                        });

            var serviceResponse = await _invitationService.SearchUser(
                new SearchUserRequst
                {
                    Query = query
                });

            if (serviceResponse.Status.Equals(SearchUserServiceResponseStatus.InternalError))
                return StatusCode(StatusCodes.Status500InternalServerError,
                       new SearchUserResponse
                       {
                           Message = serviceResponse.Status
                       });

            List<MinimalValueUserForResponseDto> users = [];
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
                Message = serviceResponse.Status,
                Results = users
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> InviteEmployee([FromBody] InviteEmployeeRequest request)
        {
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest,
                        new InviteEmployeeResponse
                        {
                            Message = "InviteDetailsIsRequired!"
                        });

            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("Id")).Value;

            var serviceResponse = await _invitationService.InviteEmployee(request, userId);

            if (serviceResponse.Status.Equals(InviteEmployeeServiceResponseStatus.InternalError))
                return StatusCode(StatusCodes.Status500InternalServerError,
                        new InviteEmployeeResponse
                        {
                            Message = serviceResponse.Status,
                        });

            if (serviceResponse.Status.Equals(InviteEmployeeServiceResponseStatus.UserNotExists))
                return StatusCode(StatusCodes.Status400BadRequest,
                        new InviteEmployeeResponse
                        {
                            Message = serviceResponse.Status,
                        });

            return Ok(new InviteEmployeeResponse
            {
                Message = serviceResponse.Status
            });
        }

        [Authorize]
        [HttpPost("accept")]
        public async Task<IActionResult> Accept([FromBody] AcceptInvitationRequest request)
        {
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest,
                       new AcceptInvitationResponse
                       {
                           Message = "ActionDetailsIsRequired!"
                       });

            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("Id")).Value;

            var serviceResponse = await _invitationService.AcceptOrganizationInvitation(request, userId);

            if (serviceResponse.Status.Equals(AcceptOrganizationInvitationServiceResponseStatus.InternalError))
                return StatusCode(StatusCodes.Status500InternalServerError,
                       new AcceptInvitationResponse
                       {
                           Message = serviceResponse.Status
                       });

            if (serviceResponse.Status.Equals(AcceptOrganizationInvitationServiceResponseStatus.NotificationNotExists))
                return StatusCode(StatusCodes.Status400BadRequest,
                       new AcceptInvitationResponse
                       {
                           Message = serviceResponse.Status
                       });

            return Ok(new AcceptInvitationResponse
            {
                Message = serviceResponse.Status
            });
        }

        [Authorize]
        [HttpPost("reject")]
        public async Task<IActionResult> Reject([FromBody] RejectInvitationRequest request)
        {
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest,
                       new RejectInvitationRespons
                       {
                           Message = "ActionDetailsIsRequired!"
                       });

            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("Id")).Value;

            var serviceResponse = await _invitationService.RejectOrganizationInvitation(request, userId);

            if (serviceResponse.Status.Equals(RejectOrganizationInvitationServiceResponseStatus.NotificationNotExists))
                return StatusCode(StatusCodes.Status400BadRequest,
                       new RejectInvitationRespons
                       {
                           Message = serviceResponse.Status
                       });

            if (serviceResponse.Status.Equals(RejectOrganizationInvitationServiceResponseStatus.InternalError))
                return StatusCode(StatusCodes.Status500InternalServerError,
                       new RejectInvitationRespons
                       {
                           Message = serviceResponse.Status
                       });

            return Ok(new RejectInvitationRespons
            {
                Message = serviceResponse.Status,
            });
        }
    }
}
