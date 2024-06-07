using Domain.Models.ApiModels.Organization.Response;
using Domain.Models.ApiModels.User.Request;
using Domain.Models.ApiModels.User.Response;
using Domain.Models.Dtos.User;
using Domain.Models.ServiceResponses.User;
using Domain.Services.ApiServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProjectManagementSystem.Controllers.User
{
    /// <summary>
    /// Provides UserService using REST-ful services
    /// </summary>
    /// <param name="userservice"></param>
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

            var serviceResponse = await _userService
                .GetUserDetails(userId);

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
                    AvatarUrl = serviceResponse.AvatarUrl
                }
            });
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateUserProfile(UpdateUserProfileRequest request)
        {
            var userId = HttpContext.User.Claims
               .FirstOrDefault(c => c.Type.Equals("Id")).Value;

            var serviceResponse = await _userService
                .UpdateUserProfile(request, userId);

            if (serviceResponse.Status == UpdateUserProfileServiceResponseStatus.AccessDenied||
                serviceResponse.Status == UpdateUserProfileServiceResponseStatus.UserNotExists)
                return StatusCode(StatusCodes.Status400BadRequest,
                        new UpdateUserProfileResponse
                        {
                            Message = serviceResponse.Status
                        });

            if (serviceResponse.Status == UpdateUserProfileServiceResponseStatus.InternalError)
                return StatusCode(StatusCodes.Status500InternalServerError,
                        new UpdateUserProfileResponse
                        {
                            Message = serviceResponse.Status
                        });

            return Ok(new UpdateUserProfileResponse
            {
                Message = serviceResponse.Status
            });                    
        }
    }
}
