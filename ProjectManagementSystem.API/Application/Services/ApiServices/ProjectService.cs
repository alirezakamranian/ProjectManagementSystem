using Domain.Constants.Roles.OrganiationEmployees;
using Domain.Models.Dtos.Project.Request;
using Domain.Models.ServiceResponses.Project;
using Domain.Models.ServiceResponses.ProjectTask;
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
                    Status = "proccesing",
                    LeaderId = userId,
                    Creationlevel ="Pending"
                });

                
                await _context.SaveChangesAsync();

                var project = await _context.Projects.Include(p => p.ProjectMembers)
                    .FirstOrDefaultAsync(p => p.LeaderId.Equals(userId)&&
                        p.Creationlevel.Equals("Pending"));

                project.ProjectMembers.Add(new()
                {
                    Role = ProjectMemberRoles.Leader,
                    OrganizationEmployeeId = employee.Id,
                    ProjectId = project.Id
                });

                project.Creationlevel = "Final";

                await _context.SaveChangesAsync();

                return new CreateProjectServiceResponse(
                     CreateProjectServiceResponseStatus.Success);
            }
            catch (Exception ex)
            {
                _logger.LogError("CreateProjectService : {Message}", ex.Message);

                return new CreateProjectServiceResponse(
                     CreateProjectServiceResponseStatus.InternalError);
            }
        }

        public async Task<GetProjectServiceResponse> GetProject(GetProjectRequest request, string userId)
        {
            try
            {
                var project = await _context.Projects
                    .AsNoTracking().Include(p=>p.ProjectMembers)
                        .Include(p => p.ProjectTaskLists)
                            .ThenInclude(tl => tl.ProjectTasks)
                                .FirstOrDefaultAsync(p => p.Id.ToString()
                                    .Equals(request.ProjectId));

                if (project == null)
                    return new GetProjectServiceResponse(
                         GetProjectServiceResponseStatus.ProjectNotExists);

                var org = await _context.Organizations.Include(o => o.OrganizationEmployees)
                    .AsNoTracking().FirstOrDefaultAsync(o => o.Id
                         .Equals(project.OrganizationId));

                var orgEmployee = org.OrganizationEmployees
                    .FirstOrDefault(e=>e.UserId.Equals(userId));

                if(orgEmployee == null)
                    return new GetProjectServiceResponse(
                         GetProjectServiceResponseStatus.AccessDenied);

                if (!project.ProjectMembers.Any(p =>
                        p.OrganizationEmployeeId.Equals(orgEmployee.Id)))
                    return new GetProjectServiceResponse(
                         GetProjectServiceResponseStatus.AccessDenied);

                return new GetProjectServiceResponse(
                     GetProjectServiceResponseStatus.Success)
                {
                    Project = project
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("GetProjectService : {Message}", ex.Message);

                return new GetProjectServiceResponse(
                     GetProjectServiceResponseStatus.InternalError);
            }
        }
    }
}
