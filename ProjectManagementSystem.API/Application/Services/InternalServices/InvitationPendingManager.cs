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

        /// <summary>
        /// Removes pending record from db and Adds user to organization 
        /// </summary>
        /// <param name="invitationId"></param>
        /// <param name="userId"></param>
        /// <returns>string<returns>
        public async Task<string> AcceptPending(string invitationId,string userId)
        {
            try
            {
                var pending = await _context.InvitationPendings
                .FirstOrDefaultAsync(n => n.NotificationId.ToString().Equals(invitationId));

                var organization = await _context.Organizations
                    .Include(o=>o.OrganizationEmployees)
                        .FirstOrDefaultAsync(o => o.Id.Equals(pending.OrganizationId));

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

        /// <summary>
        /// Removes pending record
        /// </summary>
        /// <param name="invitationId"></param>
        /// <param name="userId"></param>
        /// <returns>string</returns>
        public async Task<string> RejectPending(string invitationId, string userId)
        {
            try
            {
                var pending = await _context.InvitationPendings
                   .FirstOrDefaultAsync(n => n.NotificationId.ToString().Equals(invitationId));

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
