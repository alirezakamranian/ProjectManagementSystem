using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain.Services;
using Application.Services;
using Microsoft.IdentityModel.Tokens;
using Domain.Models.Dtos.Auth.Request;
using Domain.Models.Dtos.Auth.Response;
using Domain.Entities.HumanResource;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.Data;
using System.Text;
using Domain.Models.ServiceResponses.User.Auth;
namespace ProjectManagementSystem.Controllers.User
{
    [Route("api/user/auth")]
    [ApiController]
    public class AuthenticationController(IAuthenticationService authenticationService) : ControllerBase
    {
        IAuthenticationService _authenticationService = authenticationService;


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
                     new SignUpResponse
                     {
                         Status = serviceResponse.Message,
                         Message = "User Not Exists!"
                     });

            if (serviceResponse.Message == SignInServiceResponseMessages.InternalError)
                return StatusCode(StatusCodes.Status500InternalServerError,
                     new SignUpResponse
                     {
                         Status = serviceResponse.Message,
                         Message = "InternulServerError!"
                     });

            return Ok(serviceResponse);
        }
    }
}
