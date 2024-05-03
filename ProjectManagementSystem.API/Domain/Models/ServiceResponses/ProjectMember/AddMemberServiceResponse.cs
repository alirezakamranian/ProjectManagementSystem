using Domain.Entities.HumanResource;
using Domain.Models.ServiceResponses.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.ProjectMember
{
    public class AddMemberServiceResponse(string status) : ServiceResponseBase(status)
    {
        public Domain.Entities.HumanResource.ProjectMember Member { get; set; }
    }
    public class AddMemberServiceResponseStatus : ServiceResponseStatusBase
    {
        public const string AccessDenied = "AccessDenied";
        public const string EmployeeNotExists = "EmployeeNotExists";
        public const string ProjectNotExists = "OrganizationNotExists";
    }
}
