using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Dtos.ProjectTaskList.Request
{
    public class CreateTaskListRequest
    {
        public string ProjectId { get; set; }
        public string Name { get; set; }
    }
}
