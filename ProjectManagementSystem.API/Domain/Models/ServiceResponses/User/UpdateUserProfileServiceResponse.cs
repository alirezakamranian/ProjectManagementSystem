using Domain.Models.ServiceResponses.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.User
{
    public class UpdateUserProfileServiceResponse(string status):ServiceResponseBase(status)
    {
    }
    public class UpdateUserProfileServiceResponseStatus:ServiceResponseStatusBase
    {
        public const string AccessDenied = "AccessDenied";
        public const string UserNotExists = "UserNotExists";
    }
}
