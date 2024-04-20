using Domain.Models.ServiceResponses.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.ProjectTaskList
{
    public class ChangeTaskListPriorityServiceResponse(string status):ServiceResponseBase(status)
    {
    }
    public class ChangeTaskListPriorityServiceResponseStatus:ServiceResponseStatusBase
    {
        public const string TaskListNotExists = "TaskListNotExists";
        public const string AccessDenied = "AccessDenied";
        public const string InvalidPriority = "InvalidPriority";
    }
}
