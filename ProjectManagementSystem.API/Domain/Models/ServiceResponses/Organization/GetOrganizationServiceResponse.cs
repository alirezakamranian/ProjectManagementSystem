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
        public List<Entities.Project.Project> Projects { get; set; }
        public List<Domain.Entities.HumanResource.OrganizationEmployee> Employees { get; set; }
    }
    public class GetOrganizationServiceResponseStatus : ServiceResponseStatusBase
    {
        public const string OrganizationNotExists = "OrganizationNotExists";
        public const string AccessDenied = "AccessDenied";
    }
    public class ProjectForResponseDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }
    }
    public class OrganizationEmployeeForResponseDto
    {
        public string Id { get; set; }
        public OrganizationEmployeesRoles Role { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
    }
}
