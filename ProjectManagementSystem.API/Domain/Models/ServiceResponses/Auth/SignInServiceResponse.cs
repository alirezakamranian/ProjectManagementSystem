using Domain.Models.Dtos.Auth.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.Auth
{
    public class SignInServiceResponse(string message)
    {
        public string Status { get; set; } = message.ToString();
        public string Token { get; set; }
        public string RefrshToken { get; set; }
    }
    public static class SignInServiceResponseStatus
    {
        public const string Success = "Success";
        public const string InvalidUserCredentials = "InvalidUserCredentials";
        public const string InternalError = "InternalError";
    }
}
