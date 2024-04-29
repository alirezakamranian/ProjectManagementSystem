using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Dtos.Project.Request
{
    public class GetProjectRequest
    {
        [Required]
        public string ProjectId { get; set; }
    }
}
