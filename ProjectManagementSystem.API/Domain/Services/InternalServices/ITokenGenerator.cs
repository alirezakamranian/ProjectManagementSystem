using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.InternalServices
{
    public interface ITokenGenerator
    {
        public JwtSecurityToken GetToken(DateTime expireTime, IEnumerable<Claim> authClaims);
    }
}
