using Domain.Constants.Roles.OrganiationEmployees;
using Domain.Entities.HumanResource;
using Domain.Models.ServiceResponses.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.Organization
{
    public class GetOrganizationServiceResponse(string status) : ServiceResponseBase(status)
    {
        public string Name { get; set; }
        public string AvatarUrl { get; set; }
        public List<Entities.Project.Project> Projects { get; set; }
        public List<Domain.Entities.HumanResource.OrganizationEmployee> Employees { get; set; }
    }
    public class GetOrganizationServiceResponseStatus : ServiceResponseStatusBase
    {
        public const string OrganizationNotExists = "OrganizationNotExists";
        public const string AccessDenied = "AccessDenied";
    }
}
