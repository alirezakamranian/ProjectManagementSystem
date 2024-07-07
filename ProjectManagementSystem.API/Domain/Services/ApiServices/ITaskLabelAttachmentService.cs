using Domain.Models.ApiModels.TaskLabelAttachment.Request;
using Domain.Models.ServiceResponses.TaskLabelAttachment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.ApiServices
{
    public interface ITaskLabelAttachmentService
    {
        public Task<AttachTaskLabelServiceResponse> AttachTaskLabel(AttachTaskLabelRequest request, string userId);
        public Task<RemoveTaskLabelAttachmentServiceResponse> RemoveTaskLabelAttachment(RemoveTaskLabelAttachmentRequest request,string userId);
    }
}
