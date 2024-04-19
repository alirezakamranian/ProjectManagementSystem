using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Dtos.Project.Request
{
    public class CreateProjectRequest
    {
        [Required]
        public string OrganizationId { get; set; }
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }
} 
