using Domain.Models.ApiModels.Auth.Response;
using Domain.Models.ServiceResponses.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.Auth
{
    public class RefreshTokenServiceResponse(string status) : ServiceResponseBase(status)
    {
        public string Token { get; set; }
    }
    public class RefreshTokenServiceResponseStatus : ServiceResponseStatusBase
    {
        public const string InvalidRefreshToken = "InvalidRefreshToken";
    }
}

