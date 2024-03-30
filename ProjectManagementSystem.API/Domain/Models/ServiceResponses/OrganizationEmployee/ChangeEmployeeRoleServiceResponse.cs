using Domain.Models.ServiceResponses.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.OrganizationEmployee
{
    public class ChangeEmployeeRoleServiceResponse(string status) : ServiceResponseBase(status)
    {
    }
    public class ChangeEmployeeRoleServiceResponseStatus : ServiceResponseStatusBase
    {
        public const string AccessDenied = "AccessDenied";
        public const string OrganizationNotExists = "OrganizationNotExists";
        public const string EmployeeNotExists = "EmployeeNotExists";
    }
}
