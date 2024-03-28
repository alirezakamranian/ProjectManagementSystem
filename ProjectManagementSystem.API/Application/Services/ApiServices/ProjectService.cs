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

        public async Task<CreateProjectServiceResponse> CreateProject(CreateProjectRequest request, string email)
        {
            try
            {
                var org = await _context.Organizations
                .Include(o => o.Projects)
                    .Include(o => o.OrganizationEmployees)
                        .FirstOrDefaultAsync(o => o.Id.Equals(request.OrganizationId));
                if (org == null)
                    return new CreateProjectServiceResponse(
                         CreateProjectServiceResponseStatus.OrganizationNotExists);

                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == email);
                if (!org.OrganizationEmployees
                    .Where(e => e.UserId == user.Id)
                    .Any(o => o.Role == OrganizationEmployeesRoles.Admin))
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
