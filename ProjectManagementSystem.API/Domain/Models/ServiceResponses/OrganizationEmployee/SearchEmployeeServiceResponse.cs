using Domain.Entities.HumanResource;
using Domain.Models.ServiceResponses.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.OrganizationEmployee
{
    public class SearchEmployeeServiceResponse(string status):ServiceResponseBase(status)
    {
        public List <Domain.Entities.HumanResource.OrganizationEmployee> Emloyees { get; set; }
    }
    public class SearchEmployeeServiceResponseStatus:ServiceResponseStatusBase
    {
        public const string AccessDenied = "AccessDenied";
    }
}
