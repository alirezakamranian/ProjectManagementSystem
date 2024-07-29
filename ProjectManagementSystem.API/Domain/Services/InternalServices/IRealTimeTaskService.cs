using Domain.Models.Dtos.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.InternalServices
{
    public interface IRealTimeTaskService
    {
        public Task SendUpdate(ProjectTaskForResponseDto task, string projectId);
        public Task SendDelete(string taskId, string projectId);
        public Task SendChangeTaskList(string taskId, string projectId);
    }
}
