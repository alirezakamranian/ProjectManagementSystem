using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ApiModels.TaskComment.Request
{
    public class GetTaskCommentsRequest
    {
        public string TaskId { get; set; }
    }
}
