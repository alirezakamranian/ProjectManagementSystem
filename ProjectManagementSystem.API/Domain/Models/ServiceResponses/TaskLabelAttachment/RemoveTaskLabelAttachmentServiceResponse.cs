using Domain.Models.ServiceResponses.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.TaskLabelAttachment
{
    public class RemoveTaskLabelAttachmentServiceResponse(string status) : ServiceResponseBase(status)
    {
    }
    public class RemoveTaskLabelAttachmentServiceResponseStatus : ServiceResponseStatusBase
    {
        public const string AccessDenied = "AccessDenied";
        public const string TaskNotExists = "TaskNotExists";
    }
}
