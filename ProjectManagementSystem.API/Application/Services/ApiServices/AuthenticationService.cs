using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Domain.Models.ApiModels.Auth.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Domain.Services.InternalServices;
using Application.Services.InternalServices;
using Domain.Services.ApiServices;
using Domain.Models.ServiceResponses.Auth;
using Microsoft.Extensions.Logging;
using Domain.Entities.HumanResource;
using Domain.Models.ApiModels.Auth.Request;
namespace Application.Services.ApiServices
{
    public class AuthenticationService(UserManager<ApplicationUser> userManager,
        ITokenGenerator tokenGenerator,
        DataContext context,
        ILogger<AuthenticationService> logger) : IAuthenticationService
    {

        private readonly DataContext _context = context;
        private readonly ILogger<AuthenticationService> _logger = logger;
        private readonly ITokenGenerator _tokenGenerator = tokenGenerator;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        /// <summary>
        /// Registers user by email,fullname and selected pass
        /// </summary>
        /// <param name="request"></param>
        /// <returns>SignUpServiceResponse</returns>
        public async Task<SignUpServiceResponse> SignUpUser(SignUpRequest request)
        {
            try
            {
                var emailExists = await _userManager
                    .FindByEmailAsync(request.Email);

                if (emailExists != null)
                    return new SignUpServiceResponse(
                         SignUpServiceResponseStatus.EmailExists);

                ApplicationUser user = new()
                {
                    Email = request.Email.ToLower(),
                    SecurityStamp = Guid.NewGuid().ToString(),
                    FullName = request.FullName,
                    UserName = request.Email.ToLower()
                };

                var result = await _userManager
                    .CreateAsync(user, request.Password);

                if (!result.Succeeded)
                {
                    string errors = "";
                    foreach (var error in result.Errors)
                    {
                        errors += $"  {error.Description}";
                    }
                    return new SignUpServiceResponse(
                         SignUpServiceResponseStatus.CreationFaild)
                    {
                        Message = errors
                    };
                }

                _logger.LogInformation(
                    "New User registerd:" +
                    " Name: {FullName}" +
                    " Email: {Email}",
                    request.FullName,
                    request.Email);

                return new SignUpServiceResponse(
                     SignUpServiceResponseStatus.Success);
            }
            catch (Exception ex)
            {
                _logger.LogError("SignUpService : {Message}", ex.Message);

                return new SignUpServiceResponse(
                     SignUpServiceResponseStatus.InternalError);
            }
        }

        /// <summary>
        /// SignIns user by email and pass
        /// </summary>
        /// <param name="request"></param>
        /// <returns>SignInServiceResponse</returns>
        public async Task<SignInServiceResponse> SignInUser(SignInRequest request)
        {

            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);

                if (!(user == null) && await _userManager
                    .CheckPasswordAsync(user, request.Password))
                {
                    var userRoles = await _userManager
                        .GetRolesAsync(user);

                    var authClaims = new List<Claim>
                    {
                        new("Id", user.Id)
                    };

                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    var token = new JwtSecurityTokenHandler()
                         .WriteToken(_tokenGenerator
                            .GetToken(DateTime.Now.AddHours(1), authClaims));

                    var refreshToken = new JwtSecurityTokenHandler()
                           .WriteToken(_tokenGenerator
                               .GetToken(DateTime.Now.AddMonths(1), authClaims));

                    var oldRefreshToken = await _context.UserTokens
                        .FirstOrDefaultAsync(t => t.UserId.Equals(user.Id));

                    if (oldRefreshToken != null)
                        _context.Remove(oldRefreshToken);

                    _context.UserTokens.Add(new IdentityUserToken<string>
                    {
                        UserId = user.Id,
                        Value = refreshToken,
                        LoginProvider = "PmsAccountCenter",
                        Name = "RefreshToken"
                    });

                    await _context.SaveChangesAsync();

                    return new SignInServiceResponse(
                         SignInServiceResponseStatus.Success)
                    {
                        Token = token,
                        RefreshToken = refreshToken
                    };
                }

                return new SignInServiceResponse(
                     SignInServiceResponseStatus.InvalidUserCredentials);
            }
            catch (Exception ex)
            {
                _logger.LogError("SignInService : {Message}", ex.Message);

                return new SignInServiceResponse(
                     SignInServiceResponseStatus.InternalError);
            }
        }

        /// <summary>
        /// Refreshs token by given refreshToken in Sign-in proccess
        /// </summary>
        /// <param name="request"></param>
        /// <returns>RefreshTokenServiceResponse</returns>
        public async Task<RefreshTokenServiceResponse> RefreshToken(RefreshTokenRequest request)
        {
            try
            {
                var refreshToken = await _context.UserTokens
                    .FirstOrDefaultAsync(t => t.Value == request.RefreshToken);

                if (refreshToken != null)
                {
                    var user = await _userManager.FindByIdAsync(refreshToken.UserId);

                    var userRoles = await _userManager
                        .GetRolesAsync(user);

                    var authClaims = new List<Claim>
                    {
                         new("Id", user.Id)
                    };

                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    return new RefreshTokenServiceResponse(
                         RefreshTokenServiceResponseStatus.Success)
                    {
                        Token = new JwtSecurityTokenHandler()
                            .WriteToken(_tokenGenerator
                                .GetToken(DateTime.Now.AddHours(1), authClaims))
                    };
                }
                return new RefreshTokenServiceResponse(
                     RefreshTokenServiceResponseStatus.InvalidRefreshToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("RefreshTokenService : {Message}", ex.Message);

                return new RefreshTokenServiceResponse(
                     RefreshTokenServiceResponseStatus.InternalError);
            }
        }
    }
}
