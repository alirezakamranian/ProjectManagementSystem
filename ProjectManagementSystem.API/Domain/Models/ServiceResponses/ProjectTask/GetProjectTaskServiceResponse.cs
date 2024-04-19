using Domain.Entities.Project.ProjectTask;
using Domain.Models.ServiceResponses.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.ProjectTask
{
    public class GetProjectTaskServiceResponse(string status):ServiceResponseBase(status)
    {
        public Domain.Entities.Project.ProjectTask.ProjectTask Task { get; set; }
    }
    public class GetProjectTaskServiceResponseStatus:ServiceResponseStatusBase
    {
        public const string TaskNotExists = "TaskNotExists"; 
            public const string AccessDenied = "AccessDenied";
    }
}
