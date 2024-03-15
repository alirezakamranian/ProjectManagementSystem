using Domain.Models.Dtos.Auth.Request;
using Domain.Models.ServiceResponses.User.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.ApiServices
{
    public interface IAuthenticationService
    {
        public Task<SignUpServiceResponse> SignUpUser(SignUpRequest request);
        public Task<SignInServiceResponse> SignInUser(SignInRequest request);
        public Task<RefreshTokenServiceResponse> RefreshToken(RefreshTokenRequest request);
    }
}
