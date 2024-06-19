using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ApiModels.TaskAssignment.Request
{
    public class AssignTaskRequest
    {
        [Required]
        public string TaskId { get; set; }
        [Required]
        public string MemberId { get; set; }
        public string Description { get; set; }
    }
}
