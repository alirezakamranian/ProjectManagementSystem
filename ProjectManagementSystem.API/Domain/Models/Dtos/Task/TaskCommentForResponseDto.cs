using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Dtos.Task
{
    public class TaskCommentForResponseDto
    {
        public string Id { get; set; }
        public string TaskId { get; set; }
        public string MemberId { get; set; }
        public string Text { get; set; }
    }
}
