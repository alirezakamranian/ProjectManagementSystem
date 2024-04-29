using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Dtos.ProjectTask.Request
{
    public class GetProjectTaskRequest
    {
        [Required]
        public string TaskId { get; set; }
    }
}
