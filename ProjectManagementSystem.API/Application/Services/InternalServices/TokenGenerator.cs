using Domain.Services.InternalServices;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.InternalServices
{
    public class TokenGenerator(IConfiguration configuration):ITokenGenerator
    {
        private readonly IConfiguration _configuration = configuration;


        public JwtSecurityToken GetToken(DateTime expireTime,IEnumerable<Claim> authClaims) 
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthOptions:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["AuthOptions:IssuerAudience"],
                audience: _configuration["AuthOptions:IssuerAudience"],
                expires: expireTime,
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
}
