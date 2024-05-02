using Domain.Models.Dtos.Project;
using Domain.Models.ServiceResponses.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Dtos.Organization
{
    public class OrganizationForResponsteDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string LeaderId { get; set; }
        public List<MinimumValueProjectDto> Projects { get; set; }
    }
}
