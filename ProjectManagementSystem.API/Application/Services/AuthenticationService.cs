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
using Domain.Services.InternalServices;
using Application.Services.InternalServices;
namespace Application.Services
{
    public class AuthenticationService(UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ITokenGenerator tokenGenerator) : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly ITokenGenerator _tokenGenerator = tokenGenerator;


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
                        new("Name", user.UserName),
                        new("Email", user.Email)
                    };

                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    return new SignInServiceResponse(SignInServiceResponseMessages.Success)
                    {
                            Token = new JwtSecurityTokenHandler().WriteToken(_tokenGenerator.GetToken(DateTime.Now.AddHours(1), authClaims)),
                            RefrshToken = new JwtSecurityTokenHandler().WriteToken(_tokenGenerator.GetToken(DateTime.Now.AddMonths(1), authClaims))   
                    };
                }

                return new SignInServiceResponse(SignInServiceResponseMessages.InvalidUserCredentials);
            }
            catch
            {
                return new SignInServiceResponse(SignInServiceResponseMessages.InternalError);
            }
        }

        public async Task<RefreshTokenServiceResponse> RefreshToken(RefreshTokenRequest request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);

                if (user != null)
                {
                    var userRoles = await _userManager.GetRolesAsync(user);

                    var authClaims = new List<Claim>
                    {
                         new("Name", user.UserName),
                         new("Email", user.Email)
                    };

                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    return new RefreshTokenServiceResponse(RefreshTokenServiceResponseMessages.Success)
                    {
                        Token = new JwtSecurityTokenHandler()
                        .WriteToken(_tokenGenerator.GetToken(DateTime.Now.AddHours(1), authClaims))
                    };
                }
                return new RefreshTokenServiceResponse(RefreshTokenServiceResponseMessages.ProcessFaild);
            }
            catch
            {
                return new RefreshTokenServiceResponse(RefreshTokenServiceResponseMessages.InternalError);
            }

        }
    }
}
