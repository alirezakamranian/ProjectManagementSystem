using Domain.Models.ServiceResponses.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.TaskAssignment
{
    public class RemoveTaskAssignmentServiceResponse(string status):ServiceResponseBase(status)
    {
    }
    public class RemoveTaskAssignmentServiceResponseStatus:ServiceResponseStatusBase
    {
        public const string AccessDenied = "AccessDenied";
        public const string AssignmentNotExists = "TaskHasNoAssignment";
        public const string TaskNotExists = "TaskNotExists";
    }
}
