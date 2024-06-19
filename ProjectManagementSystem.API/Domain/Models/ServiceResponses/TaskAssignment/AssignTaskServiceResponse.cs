using Domain.Models.ServiceResponses.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.TaskAssignment
{
    public class AssignTaskServiceResponse(string status) : ServiceResponseBase(status)
    {
    }
    public class AssignTaskServiceResponseStatus : ServiceResponseStatusBase
    {
        public const string AccessDenied = "AccessDenied";
        public const string MemberNotExists = "MemberNotExists";
        public const string TaskNotExists = "TaskNotExists";
    }
}
