using Domain.Constants.AuthorizationResponses;
using Domain.Constants.Roles.OrganiationEmployees;
using Domain.Entities.Project;
using Domain.Models.ServiceResponses.ProjectTaskList;
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
    public class AuthorizationService(DataContext context) : IAuthorizationService
    {

        private readonly DataContext _context = context;

        public async Task<AuthorizationResponse> AuthorizeByProjectId(Guid projectId, string userId)
        {
            var project = _context.Projects
                .Include(p => p.ProjectMembers)
                    .AsNoTracking().FirstOrDefault(p => 
                        p.Id.Equals(projectId));

            var org = await _context.Organizations.AsNoTracking()
                   .Include(o => o.OrganizationEmployees)
                       .FirstOrDefaultAsync(o => o.Id
                           .Equals(project.OrganizationId));

            var employee = org.OrganizationEmployees
                .FirstOrDefault(e => e.UserId.Equals(userId));

            if (employee == null)
                return AuthorizationResponse.Deny;

            if (!project.ProjectMembers.Any(m =>
                m.OrganizationEmployeeId.Equals(employee.Id) &&
                    (m.Role.Equals(ProjectMemberRoles.Leader) ||
                        m.Role.Equals(ProjectMemberRoles.Admin) ||
                            m.Role.Equals(ProjectMemberRoles.Modrator))))
                return AuthorizationResponse.Deny;

            return AuthorizationResponse.Allow;
        }
    }
}
