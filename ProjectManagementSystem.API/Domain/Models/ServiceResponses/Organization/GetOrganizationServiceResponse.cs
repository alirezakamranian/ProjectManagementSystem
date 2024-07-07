using Domain.Constants.Roles.OrganiationEmployees;
using Domain.Entities.HumanResource;
using Domain.Models.Dtos.Organization;
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
        public string Description { get; set; }
        public string AvatarUrl { get; set; }
        public List<GetOrgProjectForResponseDto> Projects { get; set; }
        public List<OrganizationEmployeeForResponseDto> Employees { get; set; }
    }
    public class GetOrganizationServiceResponseStatus : ServiceResponseStatusBase
    {
        public const string OrganizationNotExists = "OrganizationNotExists";
        public const string AccessDenied = "AccessDenied";
    }
}
