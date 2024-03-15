
using Domain.Models.ServiceResponses.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.Auth
{
    public class SignUpServiceResponse(string status): ServiceResponseBase(status)
    {
        public string Message { get; set; }
    }
    public class SignUpServiceResponseStatus:ServiceResponseStatusBase
    {
        public const string EmailExists = "EmailExists";
        public const string CreationFaild = "CreationFaild";
    }
}
