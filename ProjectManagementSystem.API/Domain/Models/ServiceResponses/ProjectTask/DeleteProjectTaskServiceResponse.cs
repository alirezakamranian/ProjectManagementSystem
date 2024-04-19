using Domain.Models.ServiceResponses.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.ProjectTask
{
    public class DeleteProjectTaskServiceResponse(string status):ServiceResponseBase(status)
    {
    }
    public class DeleteProjectTaskServiceResponseStatus:ServiceResponseStatusBase
    {
        public const string AccessDenied = "AccessDenied"; 
        public const string TaskNotExists = "TaskNotExists";
    }
}
