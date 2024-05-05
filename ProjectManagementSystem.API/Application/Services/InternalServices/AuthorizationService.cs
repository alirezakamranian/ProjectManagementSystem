using Application.Services.ApiServices;
using Domain.Constants.AuthorizationResponses;
using Domain.Constants.Roles.OrganiationEmployees;
using Domain.Entities.Project;
using Domain.Models.ServiceResponses.Auth;
using Domain.Models.ServiceResponses.ProjectTaskList;
using Domain.Services.InternalServices;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.InternalServices
{
    public class AuthorizationService(DataContext context,
        ILogger<AuthorizationService> logger) : IAuthorizationService
    {

        private readonly DataContext _context = context;
        private readonly ILogger<AuthorizationService> _logger = logger;

        public async Task<AuthorizationResponse> AuthorizeByOrganizationId(Guid orgId, string userId, params OrganizationEmployeesRoles[] deniedRoles)
        {
            try
            {
                var employee = await _context.OrganizationEmployees
               .AsNoTracking().FirstOrDefaultAsync(e =>
                   e.UserId.Equals(userId) &&
                       e.OrganizationId.Equals(orgId));

                if (employee == null)
                    return AuthorizationResponse.Deny;

                if (deniedRoles.Length.Equals(0))
                    return AuthorizationResponse.Allow;

                foreach (var r in deniedRoles)
                {
                    if (employee.Role.Equals(r))
                        return AuthorizationResponse.Deny;
                }

                return AuthorizationResponse.Allow;
            }
            catch (Exception ex)
            {
                _logger.LogError("AuthorizeByOrganizationIdService : {Message}", ex.Message);

                return AuthorizationResponse.Deny;
            }
        }

        public async Task<AuthorizationResponse> AuthorizeByProjectId(Guid projectId, string userId, params ProjectMemberRoles[] deniedRoles)
        {
            try
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

                var isProjectMember = project.ProjectMembers.Any(m =>
                    m.OrganizationEmployeeId.Equals(employee.Id));

                if (!isProjectMember)
                    return AuthorizationResponse.Deny;

                if (deniedRoles.Length.Equals(0))
                    return AuthorizationResponse.Allow;

                foreach (var r in deniedRoles)
                {
                    if (project.ProjectMembers
                        .Where(m=>m.OrganizationEmployeeId
                            .Equals(employee.Id)).Any(m=> m.Role.Equals(r)))
                        return AuthorizationResponse.Deny;
                }

                return AuthorizationResponse.Allow;
            }
            catch (Exception ex)
            {
                _logger.LogError("AuthorizeByProjectIdService : {Message}", ex.Message);

                return AuthorizationResponse.Deny;
            }
        }
    }
}
