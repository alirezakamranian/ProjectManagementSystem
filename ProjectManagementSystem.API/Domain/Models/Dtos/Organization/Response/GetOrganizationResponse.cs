using Domain.Entities.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Dtos.Organization.Response
{
    public class GetOrganizationResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string Name { get; set; }
        public List<Project> Projects { get; set; }
    }
}
