using Domain.Services;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.HumanResource;
using Azure;
using Domain.Models.Dtos.Auth.Response;
using Domain.Models.Dtos.Auth.Request;
using Domain.Models.ServiceResponses.User.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
namespace Application.Services
{
    public class AuthenticationService(UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration) : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly IConfiguration _configuration = configuration;


        public async Task<SignUpServiceResponse> SignUpUser(SignUpRequest request)
        {
            try
            {
                var emailExists = await _userManager.FindByEmailAsync(request.Email);

                if (emailExists != null)
                    return new SignUpServiceResponse(SignUpServiceResponseMessages.EmailExists);

                ApplicationUser user = new()
                {
                    Email = request.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = request.Name
                };

                var result = await _userManager.CreateAsync(user, request.Password);

                if (!result.Succeeded)
                    return new SignUpServiceResponse(SignUpServiceResponseMessages.CreationFaild);

                return new SignUpServiceResponse(SignUpServiceResponseMessages.Success);
            }
            catch
            {
                return new SignUpServiceResponse(SignUpServiceResponseMessages.InternalError);
            }
        }

        public async Task<SignInServiceResponse> SignInUser(SignInRequest request)
        {

            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user != null && await _userManager.CheckPasswordAsync(user, request.Password))
                {
                    var userRoles = await _userManager.GetRolesAsync(user);

                    var authClaims = new List<Claim>
                    {
                    new Claim(ClaimTypes.Name, user.UserName),
                    };

                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthOptions:Key"]));

                    var token = new JwtSecurityToken(
                        issuer: _configuration["AuthOptions:IssuerAudience"],
                        audience: _configuration["AuthOptions:IssuerAudience"],
                        expires: DateTime.Now.AddHours(3),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                        );

                    return new SignInServiceResponse(SignInServiceResponseMessages.Success)
                    {
                        UserData = new SignInResponse() { Token = new JwtSecurityTokenHandler().WriteToken(token) }
                    };
                }

                return new SignInServiceResponse(SignInServiceResponseMessages.InvalidUserCredentials);
            }
            catch
            {
                return new SignInServiceResponse(SignInServiceResponseMessages.InternalError);
            }
        }
    }
}
