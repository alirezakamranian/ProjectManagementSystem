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
    public class GetSubscribedOrganizationsServiceResponse(string status) : ServiceResponseBase(status)
    {
        public List<OrganizationForResponsteDto> Organizations { get; set; } = null;

    }
    public class GetSubscribedOrganizationsServiceResponseStatus : ServiceResponseStatusBase
    {
    }
    public class OrganizationForResponsteDto
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public List<MinimumValueProjectDto> Projects { get; set; }
    }
    public class MinimumValueProjectDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
