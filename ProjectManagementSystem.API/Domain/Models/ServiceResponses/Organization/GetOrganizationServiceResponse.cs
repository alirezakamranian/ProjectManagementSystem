using Domain.Entities.Project;
using Domain.Models.ServiceResponses.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.Organization
{
    public class GetOrganizationServiceResponse(string status) : ServiceResponseBase(status)
    {
        public string Name { get; set; }
        public List<Project> Projects { get; set; }
    }
    public class GetOrganizationServiceResponseStatus : ServiceResponseStatusBase
    {
        public const string OrganizationNotExists = "OrganizationNotExists";
    }
}
