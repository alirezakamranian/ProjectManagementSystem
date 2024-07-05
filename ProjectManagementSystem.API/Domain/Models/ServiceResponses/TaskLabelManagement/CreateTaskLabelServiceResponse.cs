using Domain.Models.ServiceResponses.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.TaskLabelManagement
{
    public class CreateTaskLabelServiceResponse(string status) : ServiceResponseBase(status)
    {
    }
    public class CreateTaskLabelServiceResponseStatus : ServiceResponseStatusBase
    {
        public const string ProjectNotExists = "ProjectNotExists";
        public const string AccessDenied = "AccessDenied";
    }
}
