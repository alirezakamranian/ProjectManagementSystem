
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.User.Auth
{
    public class SignUpServiceResponse(string message)
    {
        public string Message { get; set; } = message;
    }
    public static class SignUpServiceResponseMessages
    {
        public const string Success = "Success";
        public const string EmailExists = "EmailExists";
        public const string CreationFaild = "CreationFaild";
        public const string InternalError = "InternalError";
    }
}
