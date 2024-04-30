using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ApiModels.ProjectTask.Request
{
    public class ChangeProjectTasksTaskListRequest
    {
        [Required]
        public string TaskId { get; set; }
        [Required]
        public string TargetTaskListId { get; set; }
        [Required]
        public int TargetPriority { get; set; }
    }
}
