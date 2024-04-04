using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain.Models.Dtos.Auth.Response;
using Microsoft.AspNetCore.Authorization;
using Domain.Services.ApiServices;
using Domain.Models.ServiceResponses.Auth;
using Domain.Models.Dtos.Auth.Request;
using Serilog;
namespace ProjectManagementSystem.Controllers.Authentication
{
    [Route("api/user/auth")]
    [ApiController]
    public class AuthenticationController(IAuthenticationService authenticationService) : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService = authenticationService;

        [HttpPost]
        [Route("sign-up")]
        public async Task<IActionResult> Register([FromBody] SignUpRequest request)
        {
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest,
                   new SignUpResponse
                   {
                       Message = "UserDetailsAreInvalid!"
                   });

            var serviceResponse = await _authenticationService.SignUpUser(request);

            if (serviceResponse.Status.Equals(SignUpServiceResponseStatus.EmailExists) ||
               serviceResponse.Status.Equals(SignUpServiceResponseStatus.CreationFaild))
                return StatusCode(StatusCodes.Status400BadRequest,
                    new SignUpResponse
                    {
                        Message = serviceResponse.Status
                    });

            if (serviceResponse.Status.Equals(SignUpServiceResponseStatus.InternalError))
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new SignUpResponse
                    {
                        Message = serviceResponse.Status
                    });

            return Ok(new SignUpResponse
            {
                Message = serviceResponse.Status
            });
        }

        [HttpPost]
        [Route("sign-in")]
        public async Task<IActionResult> Login([FromBody] SignInRequest request)
        {
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest,
                   new SignInResponse
                   {
                       Message = "UserDetailsAreRequired!"
                   });

            var serviceResponse = await _authenticationService.SignInUser(request);

            if (serviceResponse.Status.Equals(SignInServiceResponseStatus.InvalidUserCredentials))
                return StatusCode(StatusCodes.Status400BadRequest,
                     new SignInResponse
                     {
                         Message = serviceResponse.Status
                     });

            if (serviceResponse.Status.Equals(SignInServiceResponseStatus.InternalError))
                return StatusCode(StatusCodes.Status500InternalServerError,
                     new SignInResponse
                     {
                         Message = serviceResponse.Status
                     });

            return Ok(new SignInResponse
            {
                Message = serviceResponse.Status,
                Token = serviceResponse.Token,
                RefrshToken = serviceResponse.RefrshToken
            });
        }

        [HttpPost]
        [Authorize]
        [Route("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("Id")).Value;

            var serviceResponse = await _authenticationService.RefreshToken(new RefreshTokenRequest(userId));

            if (serviceResponse.Status.Equals(RefreshTokenServiceResponseStatus.ProcessFaild))
                return StatusCode(StatusCodes.Status400BadRequest,
                     new RefreshTokenResponse
                     {
                         Message = serviceResponse.Status
                     });

            if (serviceResponse.Status.Equals(RefreshTokenServiceResponseStatus.InternalError))
                return StatusCode(StatusCodes.Status500InternalServerError,
                     new RefreshTokenResponse
                     {
                         Message = serviceResponse.Status
                     });

            return Ok(new RefreshTokenResponse
            {
                Message = serviceResponse.Status,
                Token = serviceResponse.Token
            });
        }
    }
}
