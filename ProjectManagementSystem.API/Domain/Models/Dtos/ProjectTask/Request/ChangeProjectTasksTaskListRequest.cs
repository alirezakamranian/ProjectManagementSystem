using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Dtos.ProjectTask.Request
{
    public class ChangeProjectTasksTaskListRequest
    {
        public string TaskId { get; set; }
        public string TargetTaskListId { get; set; }
        public int TargetPriority { get; set; }
    }
}
