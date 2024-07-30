using Domain.Models.Dtos.Task;
using Domain.Models.InternalSerives.RealTimeTask.Response;
using Domain.Services.InternalServices;
using Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.InternalServices
{
    public class RealTimeTaskService(IHubContext<NotifHub> hub) : IRealTimeTaskService
    {

        private readonly IHubContext<NotifHub> _hub = hub;

        public async Task SendChangeTaskList(ChangeTaskListUpdateMessage message, string projectId)
        {
            await _hub.Clients.Group(projectId)
                .SendAsync("ReciveChangeTaskListUpdate",message);
        }

        public Task SendCreate(MinimumValueProjectTaskDto task, string projectId)
        {
            throw new NotImplementedException();
        }

        public Task SendDelete(string taskId, string projectId)
        {
            throw new NotImplementedException();
        }
    }
}
