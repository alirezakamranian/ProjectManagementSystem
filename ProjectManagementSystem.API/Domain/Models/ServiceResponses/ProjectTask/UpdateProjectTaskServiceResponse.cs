using Domain.Models.ServiceResponses.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.ProjectTask
{
    public class UpdateProjectTaskServiceResponse(string status):ServiceResponseBase(status)
    {
    }
    public class UpdateProjectTaskServiceResponseStatus:ServiceResponseStatusBase
    {
        public const string TaskNotExists = "TaskNotExists";
        public const string AccessDenied = "AccessDenied";
    }
}
