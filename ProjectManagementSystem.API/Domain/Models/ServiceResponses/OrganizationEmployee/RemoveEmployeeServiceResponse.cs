using Domain.Models.ServiceResponses.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.OrganizationEmployee
{
    public class RemoveEmployeeServiceResponse(string status):ServiceResponseBase(status)
    {
    }
    public class RemoveEmployeeServiceResponseStatus:ServiceResponseStatusBase
    {
        public const string AccessDenied = "AccessDenied";
        public const string EmployeeNotExists = "EmployeeNotExists";
        public const string EmployeeIsBusy = "EmployeeIsBusy";
    }
}
