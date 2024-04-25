using Domain.Models.ServiceResponses.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.ProjectTask
{
    public class ChangeProjectTasksTaskListServiceResponse(string status) : ServiceResponseBase(status)
    {
    }
    public class ChangeProjectTasksTaskListServiceResponseStatus : ServiceResponseStatusBase
    {
        public const string TaskNotExists = "TaskNotExists";
        public const string TaskListNotExists = "TaskListNotExists";
        public const string AccessDenied = "AccessDenied";
    }
}
