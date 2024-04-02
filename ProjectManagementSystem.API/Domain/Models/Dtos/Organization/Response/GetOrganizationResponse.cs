using Domain.Entities.Project;
using Domain.Models.ServiceResponses.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Dtos.Organization.Response
{
    public class GetOrganizationResponse
    {
        public string Message { get; set; }
        public string Name { get; set; }
        public List<ProjectForResponseDto> Projects { get; set; }
    }
}
