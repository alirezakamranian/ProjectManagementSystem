using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.InternalSerives.RealTimeTask.Response
{
    public class ChangeTaskListUpdateMessage
    {
        public string TaskId { get; set; }
        public string TargetTaskListId { get; set; }
        public int TargetPriority { get; set; }
    }
}
