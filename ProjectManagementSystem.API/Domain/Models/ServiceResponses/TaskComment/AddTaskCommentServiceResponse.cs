using Domain.Models.ServiceResponses.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.TaskComment
{
    public class AddTaskCommentServiceResponse(string status) : ServiceResponseBase(status)
    {
    }
    public class AddTaskCommentServiceResponseStatus : ServiceResponseStatusBase
    {
        public const string TaskNotExists = "TaskNotExists";
        public const string AccessDenied = "AccessDenied";
    }
}
