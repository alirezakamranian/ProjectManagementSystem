using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Dtos.Organization.Request
{
    public class GetOrganizationRequest
    {
        public string OrganizationId { get; set; }
        public string UserId { get; set; }
    }
}
