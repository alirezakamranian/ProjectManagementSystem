using Domain.Models.ServiceResponses.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.ProjectTaskList
{
    public class DeleteTaskListServiceResponse(string status):ServiceResponseBase(status)
    {
    }
    public class DeleteTaskListServiceResponseStatus:ServiceResponseStatusBase
    {
        public const string AccessDenied = "AccessDenied";
        public const string TaskListNotExists = "TaskListNotExists";
    }
}
