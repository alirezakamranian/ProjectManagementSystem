using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.HumanResource;
using Domain.Entities.Common;
using Domain.Models.Dtos.Organization.Response;
using Domain.Services.ApiServices;
using Infrastructure.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Domain.Constants.Notification;
using Domain.Models.ServiceResponses.Organization;
using Domain.Models.Dtos.Organization.Request;
using Domain.Constants.Roles.OrganiationEmployees;
namespace Application.Services.ApiServices
{
    public class OrganizationService(DataContext context) : IOrganizationService
    {
        private readonly DataContext _context = context;
        public async Task<CreateOrganizationServiceResponse> CreateOrganization(CreateOrganizationRequest request, string email)
        {
            try
            {
                var user = await _context.Users.Include(u => u.Organizations)
                    .FirstOrDefaultAsync(u => u.Email == email);

                user.Organizations.Add(new Organization
                {
                    Name = request.Name
                });

                await _context.SaveChangesAsync();

                var org = await _context.Organizations
                    .Include(o => o.OrganizationEmployees)
                    .FirstOrDefaultAsync(o => o.OwnerId == user.Id);

                org.OrganizationEmployees.Add(new OrganizationEmployee
                {
                    UserId = user.Id,
                    Role = OrganizationEmployeesRoles.Admin,

                });

                await _context.SaveChangesAsync();

                return new CreateOrganizationServiceResponse(
                    CreateOrganizationServiceResponseStatus.Success);
            }
            catch
            {
                return new CreateOrganizationServiceResponse(
                    CreateOrganizationServiceResponseStatus.InternalError);
            }

        }

        public async Task<UpdateOrganizationServiceResponse> UpdateOrganization(UpdateOrganizationRequest request, string email)
        {
            try
            {
                var user = await _context.Users.Include(u => u.Organizations)
                    .FirstOrDefaultAsync(u => u.Email == email);

                var org = user.Organizations.Where(o => o.Id.ToString() == request.OrganizationId)
                    .Where(o => o.OwnerId == user.Id).FirstOrDefault();

                if (org == null)
                    return new UpdateOrganizationServiceResponse(
                        UpdateOrganizationServiceResponseStatus.OrganizationNotExists);

                org.Name = request.NewName;

                await _context.SaveChangesAsync();

                return new UpdateOrganizationServiceResponse(
                    UpdateOrganizationServiceResponseStatus.Success);
            }
            catch
            {
                return new UpdateOrganizationServiceResponse(
                     UpdateOrganizationServiceResponseStatus.InternalError);
            }
        }

        public async Task<GetOrganizationServiceResponse> GetOrganization(GetOrganizationRequest request)
        {
            try
            {
                var org = await _context.Organizations.Include(o => o.Projects)
                    .Include(o => o.OrganizationEmployees)
                        .FirstOrDefaultAsync(o => o.Id.ToString() == request.OrganizationId);

                var user = await _context.Users.Include(u => u.Organizations)
                 .FirstOrDefaultAsync(u => u.Email == request.Email);

                if (org == null)
                    return new GetOrganizationServiceResponse(
                        GetOrganizationServiceResponseStatus.OrganizationNotExists);

                if (!org.OrganizationEmployees.Any(e => e.UserId == user.Id))
                    return new GetOrganizationServiceResponse(
                         GetOrganizationServiceResponseStatus.AccessDenied);

                return new GetOrganizationServiceResponse(
                     GetOrganizationServiceResponseStatus.Success)
                {
                    Name = org.Name,
                    Projects = org.Projects
                };
            }
            catch  
            {
                return new GetOrganizationServiceResponse(
                     GetOrganizationServiceResponseStatus.InternalError);
            }
        }

        public async Task<GetSubscribedOrganizationsServiceResponse> GetSubscribedOrganizations(string email)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == email);

                var memberOf = await _context.OrganizationEmployees
                    .Where(e => e.UserId == user.Id).ToListAsync();

                List<OrganizationForResponsteDto> userOrgs = [];

                foreach (var e in memberOf)
                {
                    var org = await _context.Organizations
                        .Where(o => o.Id == e.OrganizationId).FirstOrDefaultAsync();

                    userOrgs.Add(new OrganizationForResponsteDto()
                    {
                        Id = org.Id.ToString(),
                        Name = org.Name
                    });
                }

                var response = new GetSubscribedOrganizationsServiceResponse(
                    GetSubscribedOrganizationsServiceResponseStatus.Success)
                {
                    Organizations = userOrgs
                };

                return response;
            }
            catch
            {
                return new GetSubscribedOrganizationsServiceResponse(
                     GetSubscribedOrganizationsServiceResponseStatus.InternalError);
            }
        }
    }
}
