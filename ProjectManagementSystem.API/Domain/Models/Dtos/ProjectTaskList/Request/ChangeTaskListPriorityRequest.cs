using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Dtos.ProjectTaskList.Request
{
    public class ChangeTaskListPriorityRequest
    {
        public string TaskListId { get; set; }
        public int OldPriority { get; set; }
        public int NewPriority { get; set; }
    }
}
