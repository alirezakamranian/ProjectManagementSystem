using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ApiModels.ProjectTaskList.Request
{
    public class ChangeTaskListPriorityRequest
    {
        [Required]
        public string TaskListId { get; set; }

        [Required]
        public int NewPriority { get; set; }
    }
}
