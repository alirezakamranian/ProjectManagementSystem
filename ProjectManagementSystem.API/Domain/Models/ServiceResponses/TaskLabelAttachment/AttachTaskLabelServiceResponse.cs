using Domain.Models.ServiceResponses.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.TaskLabelAttachment
{
    public class AttachTaskLabelServiceResponse(string status) : ServiceResponseBase(status)
    {
    }
    public class AttachTaskLabelServiceResponseStatus : ServiceResponseStatusBase
    {
        public const string TaskNotExists = "TaskNotExists";
        public const string LabelNotExists = "LabelNotExists";
        public const string AccessDenied = "AccessDenied";
        public const string TaskAlredyHasLabel = "TaskAlredyHasLabel";
    }
}
