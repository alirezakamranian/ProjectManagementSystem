using Domain.Models.Dtos.Auth.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.Auth
{
    public class RefreshTokenServiceResponse(string message)
    {
        public string Status { get; set; } = message;
        public string Token { get; set; }
    }
    public static class RefreshTokenServiceResponseStatus
    {
        public const string Success = "Success";
        public const string ProcessFaild = "ProcessFaild";
        public const string InternalError = "InternalError";
    }
}

