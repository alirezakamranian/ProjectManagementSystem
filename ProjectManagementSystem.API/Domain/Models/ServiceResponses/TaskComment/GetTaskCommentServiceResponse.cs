using Domain.Models.Dtos.Task;
using Domain.Models.ServiceResponses.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.TaskComment
{
    public class GetTaskCommentServiceResponse(string status) : ServiceResponseBase(status)
    {
        public List<TaskCommentForResponseDto> Comments { get; set; }
    }
    public class GetTaskCommentServiceResponseStatus : ServiceResponseStatusBase
    {
        public const string AccessDenied = "AccessDenied";
        public const string TaskNotExists = "TaskNotExists";
    }
}
