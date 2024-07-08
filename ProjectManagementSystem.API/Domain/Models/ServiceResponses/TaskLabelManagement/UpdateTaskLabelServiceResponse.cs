using Domain.Models.ServiceResponses.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.TaskLabelManagement
{
    public class UpdateTaskLabelServiceResponse(string status) : ServiceResponseBase(status)
    {
    }
    public class UpdateTaskLabelServiceResponseStatus : ServiceResponseStatusBase
    {
        public const string AccessDenied = "AccessDenied";
        public const string LabelNotExists = "LabelNotExists";
    }
}
