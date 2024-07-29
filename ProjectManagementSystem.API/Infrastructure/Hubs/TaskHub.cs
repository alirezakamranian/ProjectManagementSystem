using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Hubs
{
    public class TaskHub:Hub
    {
        public override Task OnConnectedAsync()
        {
            var userId = Context.User?.Claims
                .FirstOrDefault(c => c.Type.Equals(
                    ClaimTypes.NameIdentifier))?.Value;

            Console.WriteLine($"New User connected (TaskHub) - Id :{userId}");

            return base.OnConnectedAsync();
        }

        public async Task JoinProjectGroup(string projectId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, projectId);

            Console.WriteLine($"User : {Context.ConnectionId} joined to project : {projectId}");
        }

        public async Task LeftProjectGroup(string projectId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, projectId);
        }
    }
}
