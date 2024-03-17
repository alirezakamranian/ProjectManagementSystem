using Domain.Models.ServiceResponses.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.OrganizationInvitation
{
    public class AcceptOrganizationInvitationServiceResponse(string status):ServiceResponseBase(status)
    {
    }
    public class AcceptOrganizationInvitationServiceResponseStatus:ServiceResponseStatusBase
    {
        public const string NotificationNotExists = "NotificationNotExists";
    }
}
