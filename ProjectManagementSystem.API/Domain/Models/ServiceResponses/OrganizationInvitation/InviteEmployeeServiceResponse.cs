using Domain.Models.ServiceResponses.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.OrganizationInvitation
{
    public class InviteEmployeeServiceResponse(string status) : ServiceResponseBase(status)
    {

    }
    public class InviteEmployeeServiceResponseStatus : ServiceResponseStatusBase
    {
        public const string UserNotExists = "UserNotExists";
    }
}
