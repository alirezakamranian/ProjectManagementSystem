using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ApiModels.Organization.Request
{
    public class CreateOrganizationRequest
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
