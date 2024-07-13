using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ApiModels.TaskAssignment.Request
{
    public class RemoveAssignmentRequest
    {
        [Required]
        public string TaskId { get; set; }
    }
}
