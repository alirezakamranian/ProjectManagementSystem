using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain.Services;
using Application.Services;
using Microsoft.IdentityModel.Tokens;
using Domain.Models.Dtos.Auth.Request;
namespace ProjectManagementSystem.Controllers.User
{
    [Route("api/user/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        IAuthenticationService _authenticationService;
        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp(SignUpRequest signUpRequest)
        {
             var respone = await _authenticationService.SignUpUser(signUpRequest);

            if(respone.Message== "UserAlredyExists")
                return BadRequest(respone.Message);

            if(respone.Message== "InternalServerError")
                return StatusCode(500, respone.Message);

            return Ok(respone.Message);
            
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn(SignInRequest signInRequest)
        {
            if (signInRequest.Password.IsNullOrEmpty() || signInRequest.Email.IsNullOrEmpty())
                return BadRequest("InvalidUserData");

           var respone= await _authenticationService.SignInUser(signInRequest);
            
            if(respone.Message== "InvalidUserData")
                return BadRequest("InvalidUserData");

            else if(respone.Message== "InternalServerError")
                return StatusCode(500, respone.Message);


            return Ok(respone.UserData);

        }
    }
}
