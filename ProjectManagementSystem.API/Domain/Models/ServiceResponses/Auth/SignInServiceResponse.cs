using Domain.Models.ApiModels.Auth.Response;
using Domain.Models.ServiceResponses.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.Auth
{
    public class SignInServiceResponse(string status) : ServiceResponseBase(status)
    {

        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
    public class SignInServiceResponseStatus : ServiceResponseStatusBase
    {
        public const string InvalidUserCredentials = "InvalidUserCredentials";
    }
}
