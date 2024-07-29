using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Hubs
{
    [Authorize]
    public class NotifHub:Hub
    {
        public override Task OnConnectedAsync()
        {
            var userId = Context.User?.Claims
                .FirstOrDefault(c => c.Type.Equals(
                    ClaimTypes.NameIdentifier))?.Value;

            Console.WriteLine($"New User connected (NotifHub) - Id :{userId}");

            return base.OnConnectedAsync();
        }
    }
}
