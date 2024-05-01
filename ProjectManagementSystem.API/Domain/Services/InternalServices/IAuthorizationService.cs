using Domain.Constants.AuthorizationResponses;
using Domain.Constants.Roles.OrganiationEmployees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.InternalServices
{
    public interface IAuthorizationService
    {
        public Task<AuthorizationResponse> AuthorizeByProjectId(Guid projectId, string userId, params ProjectMemberRoles[] deniedRoles);
        public Task<AuthorizationResponse> AuthorizeByOrganizationId(Guid orgId, string userId, params OrganizationEmployeesRoles[] deniedRoles);
    }
}
