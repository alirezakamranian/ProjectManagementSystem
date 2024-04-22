using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Dtos.ProjectTask.Request
{
    public class ChangeProjectTaskPriorityRequest
    {
        public string TaskId { get; set; }
        public int NewPriority { get; set; }
    }
}
