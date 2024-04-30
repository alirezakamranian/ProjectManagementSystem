using Domain.Models.ServiceResponses.ProjectMember;
using Domain.Services.ApiServices;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.HumanResource;
using Domain.Constants.Roles.OrganiationEmployees;
using Microsoft.Extensions.Logging;
using Domain.Models.ApiModels.ProjectMember.Request;
namespace Application.Services.ApiServices
{
    public class ProjectMemberService(DataContext context,
    ILogger<AuthenticationService> logger) : IProjectMemberService
    {

        private readonly DataContext _context = context;
        private readonly ILogger<AuthenticationService> _logger = logger;

        public async Task<AddMemberServiceResponse> AddMember(AddMemberRequest request, string userId)
        {
            try
            {
                var project = await _context.Projects
               .Include(p => p.ProjectMembers)
                   .FirstOrDefaultAsync(p =>
                       p.Id.ToString().Equals(request.ProjectId));

                if (project == null)
                    return new AddMemberServiceResponse(
                         AddMemberServiceResponseStatus.ProjectNotExists);

                var organization = await _context.Organizations
                    .Include(o => o.OrganizationEmployees)
                        .FirstOrDefaultAsync(o => o.Id
                            .Equals(project.OrganizationId));

                if (!project.ProjectMembers
                    .Any(pm => pm.OrganizationEmployeeId
                        .Equals(organization.OrganizationEmployees
                            .Where(e => e.UserId.ToString().Equals(userId))
                                .Select(e => e.Id).FirstOrDefault()) && (pm.Role.Equals(
                                    ProjectMemberRoles.Leader) || pm.Role.Equals(ProjectMemberRoles.Admin))))
                    return new AddMemberServiceResponse(
                         AddMemberServiceResponseStatus.AccessDenied);

                var employee = organization.OrganizationEmployees
                    .FirstOrDefault(e => e.Id.ToString().Equals(request.EmployeeId));

                if (employee == null)
                    return new AddMemberServiceResponse(
                         AddMemberServiceResponseStatus.EmployeeNotExists);

                project.ProjectMembers.Add(new()
                {
                    Role = ProjectMemberRoles.Member,
                    OrganizationEmployeeId = employee.Id
                });
                 await _context.SaveChangesAsync();

                return new AddMemberServiceResponse(
                     AddMemberServiceResponseStatus.Success);
            }
            catch (Exception ex)
            {
                _logger.LogError("AddMemberService : {Message}", ex.Message);

                return new AddMemberServiceResponse(
                     AddMemberServiceResponseStatus.InternalError);
            }

        }
    }
}
