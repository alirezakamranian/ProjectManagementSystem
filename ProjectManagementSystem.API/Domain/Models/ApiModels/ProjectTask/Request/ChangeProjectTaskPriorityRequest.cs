using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ApiModels.ProjectTask.Request
{
    public class ChangeProjectTaskPriorityRequest
    {
        [Required]
        public string TaskId { get; set; }
        [Required]
        public int NewPriority { get; set; }
    }
}
