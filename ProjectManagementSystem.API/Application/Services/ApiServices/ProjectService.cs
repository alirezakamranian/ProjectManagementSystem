using Domain.Constants.Roles.OrganiationEmployees;
using Domain.Models.Dtos.Project.Request;
using Domain.Models.ServiceResponses.Project;
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
    public class ProjectService(DataContext context) : IProjectService
    {

        private readonly DataContext _context = context;

        public async Task<CreateProjectServiceResponse> CreateProject(CreateProjectRequest request, string userId)
        {
            try
            {
                var org = await _context.Organizations
                    .Include(o => o.Projects)
                       .Include(o => o.OrganizationEmployees)
                          .FirstOrDefaultAsync(o => o.Id.ToString()
                              .Equals(request.OrganizationId));
                if (org.Equals(null))
                    return new CreateProjectServiceResponse(
                         CreateProjectServiceResponseStatus.OrganizationNotExists);

                if (!org.OrganizationEmployees
                    .Where(e => e.UserId.Equals(userId))
                    .Any(o => o.Role.Equals(OrganizationEmployeesRoles.Admin)))
                    return new CreateProjectServiceResponse(
                         CreateProjectServiceResponseStatus.AccessDenied);

                org.Projects.Add(new()
                {
                    Name = request.Name,
                    Description = request.Description,
                    DeadLine = request.DeadLine,
                    StartDate=DateTime.Now,
                    Status="proccesing"
                });

                await _context.SaveChangesAsync();

                return new CreateProjectServiceResponse(
                     CreateProjectServiceResponseStatus.Success);
            }
            catch
            {
                return new CreateProjectServiceResponse(
                     CreateProjectServiceResponseStatus.InternalError);
            }
        }
    }
}
