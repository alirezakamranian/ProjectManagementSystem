using Domain.Models.Dtos.Project;
using Domain.Models.Dtos.Task;
using Domain.Models.InternalSerives.RealTimeTask.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.InternalServices
{
    public interface IRealTimeTaskService
    {
        public Task SendDelete(string taskId, string projectId);
        public Task SendCreate(MinimumValueProjectTaskDto task, string projectId);
        public Task SendChangeTaskList(ChangeTaskListUpdateMessage message, string projectId);
    }
}
