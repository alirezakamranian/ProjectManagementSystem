using Domain.Models.ServiceResponses.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.TaskLabelManagement
{
    public class RemoveTaskLabelServiceResponse(string status):ServiceResponseBase(status)
    {
    }
    public class RemoveTaskLabelServiceResponseStatus : ServiceResponseStatusBase
    {
        public const string LabelNotExists = "LabelNotExists";
        public const string AccessDenied = "AccessDenied";
    }
}
