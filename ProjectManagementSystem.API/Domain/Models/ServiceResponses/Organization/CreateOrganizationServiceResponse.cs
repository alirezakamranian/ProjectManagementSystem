using Domain.Models.ServiceResponses.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.Organization
{
    public class CreateOrganizationServiceResponse(string status) : ServiceResponseBase(status)
    {

    }
    public class CreateOrganizationServiceResponseStatus : ServiceResponseStatusBase
    {
        public const string CreationFaild = "CreationFaild";
        public const string OrganizationExists = "OrganizationExists";
    }
}
