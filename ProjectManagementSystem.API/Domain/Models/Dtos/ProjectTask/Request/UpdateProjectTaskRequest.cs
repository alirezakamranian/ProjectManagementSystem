using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Dtos.ProjectTask.Request
{
    public class UpdateProjectTaskRequest
    {
        public string TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
