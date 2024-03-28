using Domain.Models.ServiceResponses.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.Project
{
    public class CreateProjectServiceResponse(string status) : ServiceResponseBase(status)
    {
    }
    public class CreateProjectServiceResponseStatus : ServiceResponseStatusBase
    {
        public const string OrganizationNotExists = "OrganizationNotExists";
        public const string AccessDenied = "AccessDenied";
    }
}
