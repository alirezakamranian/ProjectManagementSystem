using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.InternalServices
{
    public interface IInvitationPendingManager
    {
        public Task<string> AcceptPending(string invitationId, string userId);

        public Task<string> RejectPending(string invitationId, string userId);

    }
}
