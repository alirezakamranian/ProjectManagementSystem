using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Dtos.Organization.Request
{
    public class UpdateOrganizationRequest
    {
        public string OrganizationId { get; set; }
        public string NewName { get; set; }
    }
}
