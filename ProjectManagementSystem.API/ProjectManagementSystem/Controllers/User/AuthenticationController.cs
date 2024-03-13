using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain.Services;
using Domain.Models.Dtos.Auth.Request;
using Domain.Models.Dtos.Auth.Response;
using Domain.Models.ServiceResponses.User.Auth;
using Microsoft.AspNetCore.Authorization;
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

            if (serviceResponse.Message == SignUpServiceResponseMessages.EmailExists)
                return StatusCode(StatusCodes.Status400BadRequest,
                    new SignUpResponse
                    {
                        Status = serviceResponse.Message,
                        Message = "User already exists!"
                    });

            if (serviceResponse.Message == SignUpServiceResponseMessages.CreationFaild)
                return StatusCode(StatusCodes.Status400BadRequest,
                    new SignUpResponse
                    {
                        Status = serviceResponse.Message,
                        Message = "User creation failed! Please check user details and try again."
                    });

            if (serviceResponse.Message == SignUpServiceResponseMessages.InternalError)
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new SignUpResponse
                    {
                        Status = serviceResponse.Message,
                        Message = "InternulServerError!"
                    });

            return Ok(new SignUpResponse
            {
                Status = serviceResponse.Message,
                Message = "User created successfully!"
            });
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] SignInRequest request)
        {
            var serviceResponse=await _authenticationService.SignInUser(request);

            if (serviceResponse.Message == SignInServiceResponseMessages.InvalidUserCredentials)
                return StatusCode(StatusCodes.Status400BadRequest,
                     new SignInResponse
                     {
                         Status = serviceResponse.Message,
                         Message = "User Not Exists!"
                     });

            if (serviceResponse.Message == SignInServiceResponseMessages.InternalError)
                return StatusCode(StatusCodes.Status500InternalServerError,
                     new SignInResponse
                     {
                         Status = serviceResponse.Message,
                         Message = "InternulServerError!"
                     });

            return Ok(new SignInResponse
            {
                Status= serviceResponse.Message,
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

            if (serviceResponse.Message == RefreshTokenServiceResponseMessages.ProcessFaild)
                return StatusCode(StatusCodes.Status400BadRequest,
                     new RefreshTokenResponse
                     {
                         Status = serviceResponse.Message,
                         Message = "RefreshingFaild"
                     });

            if (serviceResponse.Message == RefreshTokenServiceResponseMessages.InternalError)
                return StatusCode(StatusCodes.Status500InternalServerError,
                     new RefreshTokenResponse
                     {
                         Status = serviceResponse.Message,
                         Message = "InternulServerError!"
                     });

            return Ok(new RefreshTokenResponse
            {
                Status= serviceResponse.Message,
                Message= "Refresh Token successfully!",
                Token = serviceResponse.Token
            });
        }
    }
}
