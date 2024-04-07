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

        public DateTime StartDate { get; set; }

        public DateTime DeadLine { get; set; }

        public DateTime EndDate { get; set; }

        public string Status { get; set; }
    }
}
