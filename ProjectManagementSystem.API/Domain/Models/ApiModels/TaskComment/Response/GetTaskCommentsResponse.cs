using Domain.Models.Dtos.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ApiModels.TaskComment.Response
{
    public class GetTaskCommentsResponse
    {
        public string Message { get; set; }
        public List<TaskCommentForResponseDto> Comments { get; set; }
    }
}
