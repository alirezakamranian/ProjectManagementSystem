using Domain.Models.ServiceResponses.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Dtos.Organization.Response
{
    public class GetSubscribedOrganizationsResponse
    {
        public string Message { get; set; }
        public List<OrganizationForResponsteDto> Organizations { get; set; }
    }
}
