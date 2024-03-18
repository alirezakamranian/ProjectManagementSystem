using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.InternalServices
{
    public interface IInvitationPendingManager
    {
        public Task<string> AcceptPending(int invitationId, string userId);

        public Task<string> RejectPending(int invitationId, string userId);

    }
}
