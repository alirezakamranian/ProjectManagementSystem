using Domain.Constants.Roles.OrganiationEmployees;
using Domain.Entities.HumanResource;
using Domain.Services.InternalServices;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.InternalServices
{
    public class InvitationPendingManager(DataContext context) : IInvitationPendingManager
    {
        private readonly DataContext _context = context;

        public async Task<string> AcceptPending(int invitationId,string userId)
        {
            try
            {
                var pending = await _context.InvitationPendings
                .FirstOrDefaultAsync(n => n.NotificationId == invitationId);

                var organization = await _context.Organizations
                    .Include(o=>o.OrganizationEmployees)
                    .FirstOrDefaultAsync(o => o.Id == pending.OrganizationId);

                organization.OrganizationEmployees.Add(new OrganizationEmployee
                {
                    UserId = userId,
                    Role = OrganizationEmployeesRoles.Member
                });

                 _context.Remove(pending);

                await _context.SaveChangesAsync();

                return "Success";
            }
            catch 
            {
                return "error";
            }

        }

        public async Task<string> RejectPending(int invitationId, string userId)
        {
            try
            {
                var pending = await _context.InvitationPendings
                   .FirstOrDefaultAsync(n => n.NotificationId == invitationId);

                _context.Remove(pending);

                await _context.SaveChangesAsync();

                return "Success";
            }
            catch 
            {
                return "error";
            }
        }
    }
}
