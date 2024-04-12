using Domain.Models.ServiceResponses.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.ProjectTaskList
{
    public class ProjectTaskListServiceResponse(string status):ServiceResponseBase(status)
    {

    }
    public class ProjectTaskListServiceResponseStatus:ServiceResponseStatusBase
    {
        public const string AccessDenied = "AccessDenied";
        public const string ProjectNotExists = "OrganizationNotExists";
    }
}
