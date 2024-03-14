
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.User.Auth
{
    public class SignUpServiceResponse(string status)
    {
        public string Message { get; set; }
        public string Status { get; set; } = status;
    }
    public static class SignUpServiceResponseStatus
    {
        public const string Success = "Success";
        public const string EmailExists = "EmailExists";
        public const string CreationFaild = "CreationFaild";
        public const string InternalError = "InternalError";
    }
}
