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
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Domain.Services.InternalServices;
using Application.Services.InternalServices;
using Domain.Services.ApiServices;
using Domain.Models.ServiceResponses.Auth;
using Domain.Models.Dtos.Auth.Request;
namespace Application.Services.ApiServices
{
    public class AuthenticationService(UserManager<ApplicationUser> userManager,
        ITokenGenerator tokenGenerator) : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ITokenGenerator _tokenGenerator = tokenGenerator;


        public async Task<SignUpServiceResponse> SignUpUser(SignUpRequest request)
        {
            try
            {
                var emailExists = await _userManager.FindByEmailAsync(request.Email);

                if (emailExists != null)
                    return new SignUpServiceResponse(SignUpServiceResponseStatus.EmailExists);

                ApplicationUser user = new()
                {
                    Email = request.Email.ToLower(),
                    SecurityStamp = Guid.NewGuid().ToString(),
                    FullName = request.FullName,
                    UserName = request.Email.ToLower()
                };

                var result = await _userManager.CreateAsync(user, request.Password);

                if (!result.Succeeded)
                {
                    string errors = "";
                    foreach (var error in result.Errors)
                    {
                        errors += $"  {error.Description}";
                    }
                    return new SignUpServiceResponse(SignUpServiceResponseStatus.CreationFaild)
                    {
                        Message = errors
                    };
                }

                return new SignUpServiceResponse(SignUpServiceResponseStatus.Success);
            }
            catch
            {
                return new SignUpServiceResponse(SignUpServiceResponseStatus.InternalError);
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
                        new("Id", user.Id)
                    };

                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    return new SignInServiceResponse(SignInServiceResponseStatus.Success)
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(_tokenGenerator.GetToken(DateTime.Now.AddHours(1), authClaims)),
                        RefrshToken = new JwtSecurityTokenHandler().WriteToken(_tokenGenerator.GetToken(DateTime.Now.AddMonths(1), authClaims))
                    };
                }

                return new SignInServiceResponse(SignInServiceResponseStatus.InvalidUserCredentials);
            }
            catch
            {
                return new SignInServiceResponse(SignInServiceResponseStatus.InternalError);
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
                         new("Id", user.Id)
                    };

                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    return new RefreshTokenServiceResponse(RefreshTokenServiceResponseStatus.Success)
                    {
                        Token = new JwtSecurityTokenHandler()
                        .WriteToken(_tokenGenerator.GetToken(DateTime.Now.AddHours(1), authClaims))
                    };
                }
                return new RefreshTokenServiceResponse(RefreshTokenServiceResponseStatus.ProcessFaild);
            }
            catch
            {
                return new RefreshTokenServiceResponse(RefreshTokenServiceResponseStatus.InternalError);
            }
        }
    }
}
