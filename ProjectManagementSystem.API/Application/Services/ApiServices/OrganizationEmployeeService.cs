using Domain.Constants.AuthorizationResponses;
using Domain.Constants.Roles.OrganiationEmployees;
using Domain.Models.ApiModels.OrganizationEmployee.Request;
using Domain.Models.ServiceResponses.OrganizationEmployee;
using Domain.Services.ApiServices;
using Domain.Services.InternalServices;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services.ApiServices
{
    public class OrganizationEmployeeService(DataContext context,
        ILogger<OrganizationEmployeeService> logger,
            IAuthorizationService authService) : IOrganizationEmployeeService
    {
        private readonly DataContext _context = context;
        private readonly ILogger<OrganizationEmployeeService> _logger = logger;
        private readonly IAuthorizationService _authService = authService;

        /// <summary>
        /// Changes OrgEmnployee role 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns>ChangeEmployeeRoleServiceResponse</returns>
        public async Task<ChangeEmployeeRoleServiceResponse> ChangeEmployeeRole(ChangeEmployeeRoleRequest request, string userId)
        {
            try
            {
                var org = await _context.Organizations
                    .Include(o => o.OrganizationEmployees)
                         .FirstOrDefaultAsync(o => o.Id.ToString()
                              .Equals(request.OrganizationId));

                if (org == null)
                    return new ChangeEmployeeRoleServiceResponse(
                         ChangeEmployeeRoleServiceResponseStatus.OrganizationNotExists);

                var authResult = await _authService
                    .AuthorizeByOrganizationId(org.Id, userId,
                        [OrganizationEmployeesRoles.Member]);

                if (authResult.Equals(AuthorizationResponse.Deny))
                    return new ChangeEmployeeRoleServiceResponse(
                         ChangeEmployeeRoleServiceResponseStatus.AccessDenied);

                var targetUser = org.OrganizationEmployees
                    .FirstOrDefault(e => e.Id.ToString()
                        .Equals(request.EmployeeId));

                if (targetUser == null)
                    return new ChangeEmployeeRoleServiceResponse(
                         ChangeEmployeeRoleServiceResponseStatus.EmployeeNotExists);

                if (request.Role
                    .Equals(OrganizationEmployeesRoles.Member))
                    targetUser.Role = OrganizationEmployeesRoles.Member;

                if (request.Role
                    .Equals(OrganizationEmployeesRoles.Admin))
                    targetUser.Role = OrganizationEmployeesRoles.Admin;

                await _context.SaveChangesAsync();


                return new ChangeEmployeeRoleServiceResponse(
                      ChangeEmployeeRoleServiceResponseStatus.Success);
            }
            catch (Exception ex)
            {
                _logger.LogError("ChangeEmployeeRoleService : {Message}", ex.Message);

                return new ChangeEmployeeRoleServiceResponse(
                     ChangeEmployeeRoleServiceResponseStatus.InternalError);
            }
        }

        /// <summary>
        /// Removes employee from Organization
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns>RemoveEmployeeServiceResponse</returns>
        public async Task<RemoveEmployeeServiceResponse> RemoveEmployee(RemoveEmployeeRequest request, string userId)
        {
            try
            {
                var employee = await _context.OrganizationEmployees
                    .FirstOrDefaultAsync(e => e.Id.ToString()
                        .Equals(request.EmployeeId));

                if (employee == null)
                    return new RemoveEmployeeServiceResponse(
                         RemoveEmployeeServiceResponseStatus.EmployeeNotExists);

                var org = await _context.Organizations
                    .Include(o => o.OrganizationEmployees)
                        .AsNoTracking().FirstOrDefaultAsync(o => o.Id
                            .Equals(employee.OrganizationId));

                var authResult = await _authService
                    .AuthorizeByOrganizationId(org.Id, userId,
                        [OrganizationEmployeesRoles.Member]);

                if (authResult.Equals(AuthorizationResponse.Deny))
                    return new RemoveEmployeeServiceResponse(
                         RemoveEmployeeServiceResponseStatus.AccessDenied);

                var con = await _context.Projects
                    .FirstOrDefaultAsync(p => p.OrganizationId
                        .Equals(org.Id) && p.LeaderId
                            .Equals(employee.Id.ToString()));

                if (con != null)
                    return new RemoveEmployeeServiceResponse(
                         RemoveEmployeeServiceResponseStatus.EmployeeIsBusy);

                _context.OrganizationEmployees.Remove(employee);

                await _context.SaveChangesAsync();

                return new RemoveEmployeeServiceResponse(
                     RemoveEmployeeServiceResponseStatus.Success);
            }
            catch (Exception ex)
            {
                _logger.LogError("RemoveEmployeeService : {Message}", ex.Message);

                return new RemoveEmployeeServiceResponse(
                     RemoveEmployeeServiceResponseStatus.InternalError);
            }
        }

        /// <summary>
        /// Used for search employees in organization
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns>SearchEmployeeServiceResponse</returns>
        public async Task<SearchEmployeeServiceResponse> SearchEmployee(SearchEmployeeRequest request, string userId)
        {
            try
            {
                var authResult = await _authService
                    .AuthorizeByOrganizationId(Guid.Parse
                        (request.OrganizationId), userId);

                if (authResult.Equals(AuthorizationResponse.Deny))
                    return new SearchEmployeeServiceResponse(
                         SearchEmployeeServiceResponseStatus.AccessDenied);

                var employees = await _context.OrganizationEmployees
                    .Include(e => e.User).AsNoTracking()
                        .Where(e => e.OrganizationId.ToString()
                            .Equals(request.OrganizationId) &&
                                (e.User.Email.Contains(request.Query
                                    .ToLower().Trim()) || e.User.FullName
                                        .ToLower().Contains(request.Query
                                            .ToLower().Trim()))).ToListAsync();

                return new SearchEmployeeServiceResponse(
                     SearchEmployeeServiceResponseStatus.Success)
                {
                    Emloyees = employees
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("SearchEmployeeService : {Message}", ex.Message);

                return new SearchEmployeeServiceResponse(
                     SearchEmployeeServiceResponseStatus.InternalError);
            }
        }
    }
}
