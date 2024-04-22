using Domain.Models.ServiceResponses.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.ProjectTask
{
    public class ChangeProjectTaskPriorityServiceResponse(string status) : ServiceResponseBase(status)
    {

    }
    public class ChangeProjectTaskPriorityServiceResponseStatus : ServiceResponseStatusBase
    {
        public const string TaskNotExists = "TaskNotExists";
        public const string AccessDenied = "AccessDenied";
        public const string InvalidPriority = "InvalidPriority";
    }
}
