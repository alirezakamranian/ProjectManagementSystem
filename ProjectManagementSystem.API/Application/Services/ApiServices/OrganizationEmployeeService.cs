using Domain.Constants.Roles.OrganiationEmployees;
using Domain.Models.ApiModels.OrganizationEmployee.Request;
using Domain.Models.ServiceResponses.OrganizationEmployee;
using Domain.Services.ApiServices;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.ApiServices
{
    public class OrganizationEmployeeService(DataContext context,
        ILogger<AuthenticationService> logger) : IOrganizationEmployeeService
    {
        private readonly DataContext _context = context;
        private readonly ILogger<AuthenticationService> _logger = logger;

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

                if (!org.OrganizationEmployees.Any(
                    e => e.UserId.Equals(userId) &&
                        e.Role.Equals(OrganizationEmployeesRoles.Admin) ||
                            e.Role.Equals(OrganizationEmployeesRoles.Leader)))
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

                if (!org.OrganizationEmployees.Any(
                    e => e.UserId.Equals(userId) &&
                        e.Role.Equals(OrganizationEmployeesRoles.Leader)))
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
    }
}
