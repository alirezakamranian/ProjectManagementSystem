using Domain.Models.Dtos.Organization.Response;
using Domain.Models.Dtos.User.Response;
using Domain.Models.ServiceResponses.User;
using Domain.Services.ApiServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProjectManagementSystem.Controllers.User
{
    [Route("user")]
    [ApiController]
    public class UserManagementController(IUserService userservice) : ControllerBase
    {
        private readonly IUserService _userService = userservice;

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUserDetails()
        {
            var userId = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type.Equals("Id")).Value;

            var serviceResponse = await _userService.GetUserDetails(userId);

            if (serviceResponse.Status == GetUserDetailsServiceResponseStatus.InternalError)
                return StatusCode(StatusCodes.Status400BadRequest,
                        new GetUserDetailsResponse
                        {
                            Message = serviceResponse.Status
                        });

            return Ok(new GetUserDetailsResponse
            {
                Message = serviceResponse.Status,
                NotificationsCount = serviceResponse.NotificationsCount,
                UserDetails = new UserForResponseDto()
                {
                    Id = serviceResponse.User.Id,
                    Email = serviceResponse.User.Email,
                    FullName = serviceResponse.User.FullName,
                    Avatar = null
                }
            });
        }
    }
}
