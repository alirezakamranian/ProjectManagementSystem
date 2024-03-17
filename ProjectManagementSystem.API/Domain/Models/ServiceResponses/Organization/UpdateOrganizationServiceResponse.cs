using Domain.Models.ServiceResponses.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.Organization
{
    public class UpdateOrganizationServiceResponse(string status) : ServiceResponseBase(status)
    {
    }
    public class UpdateOrganizationServiceResponseStatus : ServiceResponseStatusBase
    {
        public const string OrganizationNotExists = "OrganizationNotExists";
    }
}
