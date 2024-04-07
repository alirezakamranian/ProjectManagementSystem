using Domain.Constants.Roles.OrganiationEmployees;
using Domain.Models.Dtos.Project.Request;
using Domain.Models.ServiceResponses.Project;
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
    public class ProjectService(DataContext context,
        ILogger<AuthenticationService> logger) : IProjectService
    {

        private readonly DataContext _context = context;
        private readonly ILogger<AuthenticationService> _logger = logger;

        public async Task<CreateProjectServiceResponse> CreateProject(CreateProjectRequest request, string userId)
        {
            try
            {
                var org = await _context.Organizations
                    .Include(o => o.Projects).ThenInclude(p => p.ProjectMembers)
                       .Include(o => o.OrganizationEmployees)
                          .FirstOrDefaultAsync(o => o.Id.ToString()
                              .Equals(request.OrganizationId));
                if (org.Equals(null))
                    return new CreateProjectServiceResponse(
                         CreateProjectServiceResponseStatus.OrganizationNotExists);

                var employee = org.OrganizationEmployees
                    .Where(e => e.UserId.Equals(userId)).FirstOrDefault();

                if (employee == null || employee.Role
                .Equals(OrganizationEmployeesRoles.Member))
                    return new CreateProjectServiceResponse(
                         CreateProjectServiceResponseStatus.AccessDenied);

                org.Projects.Add(new()
                {
                    Name = request.Name,
                    Description = request.Description,
                    DeadLine = request.DeadLine,
                    StartDate = DateTime.Now,
                    Status = "proccesing",
                    LeaderId = userId
                });

                await _context.SaveChangesAsync();

                var project = await _context.Projects.Include(p=>p.ProjectMembers)
                .FirstOrDefaultAsync(p => p.LeaderId.Equals(userId));

                project.ProjectMembers.Add(new()
                {
                    Role = ProjectRoles.Leader,
                    OrganizationEmployeeId = employee.Id,
                    ProjectId = project.Id
                });

                await _context.SaveChangesAsync();

                return new CreateProjectServiceResponse(
                     CreateProjectServiceResponseStatus.Success);
            }
            catch (Exception ex)
            {
                _logger.LogError($"CreateProjectService : {ex.Message}");

                return new CreateProjectServiceResponse(
                     CreateProjectServiceResponseStatus.InternalError);
            }
        }
    }
}
