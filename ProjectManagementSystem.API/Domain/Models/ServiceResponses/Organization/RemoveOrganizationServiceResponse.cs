using Domain.Models.ServiceResponses.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.Organization
{
    public class RemoveOrganizationServiceResponse(string status) : ServiceResponseBase(status)
    {
    }
    public class RemoveOrganizationServiceResponseStatus : ServiceResponseStatusBase
    {
        public const string AccessDenied = "AccessDenied";
        public const string OrganizationNotExists = "OrganizationNotExists";
    }
}
