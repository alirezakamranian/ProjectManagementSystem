using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain.Models.Dtos.Auth.Response;
using Microsoft.AspNetCore.Authorization;
using Domain.Services.ApiServices;
using Domain.Models.ServiceResponses.Auth;
using Domain.Models.Dtos.Auth.Request;
namespace ProjectManagementSystem.Controllers.User
{
    [Route("api/user/auth")]
    [ApiController]
    public class AuthenticationController(IAuthenticationService authenticationService) : ControllerBase
    {
       private readonly IAuthenticationService _authenticationService = authenticationService;

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] SignUpRequest request)
        {
           var serviceResponse =await _authenticationService.SignUpUser(request);

            if (serviceResponse.Status == SignUpServiceResponseStatus.EmailExists)
                return StatusCode(StatusCodes.Status400BadRequest,
                    new SignUpResponse
                    {
                        Status = serviceResponse.Status,
                        Message = "User already exists!"
                    });

            if (serviceResponse.Status == SignUpServiceResponseStatus.CreationFaild)
                return StatusCode(StatusCodes.Status400BadRequest,
                    new SignUpResponse
                    {
                        Status = serviceResponse.Status,
                        Message = serviceResponse.Message
                    });

            if (serviceResponse.Status == SignUpServiceResponseStatus.InternalError)
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new SignUpResponse
                    {
                        Status = serviceResponse.Status,
                        Message = "InternulServerError!"
                    });

            return Ok(new SignUpResponse
            {
                Status = serviceResponse.Status,
                Message = "UserCreatedSuccessfully!"
            });
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] SignInRequest request)
        {
            var serviceResponse=await _authenticationService.SignInUser(request);

            if (serviceResponse.Status == SignInServiceResponseStatus.InvalidUserCredentials)
                return StatusCode(StatusCodes.Status400BadRequest,
                     new SignInResponse
                     {
                         Status = serviceResponse.Status,
                         Message = "UserNotExists!"
                     });

            if (serviceResponse.Status == SignInServiceResponseStatus.InternalError)
                return StatusCode(StatusCodes.Status500InternalServerError,
                     new SignInResponse
                     {
                         Status = serviceResponse.Status,
                         Message = "InternulServerError!"
                     });

            return Ok(new SignInResponse
            {
                Status= serviceResponse.Status,
                Message= "User Login successfully!",
                Token=serviceResponse.Token,
                RefrshToken=serviceResponse.RefrshToken
            });
        }


        [HttpPost]
        [Authorize]
        [Route("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var email = HttpContext.User.Claims.FirstOrDefault(c=>c.Type=="Email");

            var serviceResponse = await _authenticationService.RefreshToken(new RefreshTokenRequest(email.Value));

            if (serviceResponse.Status == RefreshTokenServiceResponseStatus.ProcessFaild)
                return StatusCode(StatusCodes.Status400BadRequest,
                     new RefreshTokenResponse
                     {
                         Status = serviceResponse.Status,
                         Message = "RefreshingFaild"
                     });

            if (serviceResponse.Status == RefreshTokenServiceResponseStatus.InternalError)
                return StatusCode(StatusCodes.Status500InternalServerError,
                     new RefreshTokenResponse
                     {
                         Status = serviceResponse.Status,
                         Message = "InternulServerError!"
                     });

            return Ok(new RefreshTokenResponse
            {
                Status= serviceResponse.Status,
                Message= "RefreshTokenSuccessfully!",
                Token = serviceResponse.Token
            });
        }
    }
}
