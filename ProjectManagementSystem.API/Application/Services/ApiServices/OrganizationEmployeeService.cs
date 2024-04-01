using Domain.Constants.Roles.OrganiationEmployees;
using Domain.Models.Dtos.OrganizationEmployee.Request;
using Domain.Models.ServiceResponses.OrganizationEmployee;
using Domain.Services.ApiServices;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.ApiServices
{
    public class OrganizationEmployeeService(DataContext context) : IOrganizationEmployeeService
    {
        public readonly DataContext _context = context;

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
                    e.Role == OrganizationEmployeesRoles.Admin))
                    return new ChangeEmployeeRoleServiceResponse(
                      ChangeEmployeeRoleServiceResponseStatus.AccessDenied);

                var targetUser = org.OrganizationEmployees
                    .FirstOrDefault(e => e.Id.ToString()
                        .Equals(request.EmployeeId));

                if (targetUser == null)
                    return new ChangeEmployeeRoleServiceResponse(
                         ChangeEmployeeRoleServiceResponseStatus.EmployeeNotExists);

                if (request.Role.Trim().ToLower()
                    .Equals(OrganizationEmployeesRoles.Member.ToLower()))
                    targetUser.Role = OrganizationEmployeesRoles.Member;

                if (request.Role.Trim().ToLower()
                    .Equals(OrganizationEmployeesRoles.Admin.ToLower()))
                    targetUser.Role = OrganizationEmployeesRoles.Admin;

                await _context.SaveChangesAsync();


                return new ChangeEmployeeRoleServiceResponse(
                      ChangeEmployeeRoleServiceResponseStatus.Success);
            }
            catch
            {
                return new ChangeEmployeeRoleServiceResponse(
                     ChangeEmployeeRoleServiceResponseStatus.InternalError);
            }
        }
    }
}
