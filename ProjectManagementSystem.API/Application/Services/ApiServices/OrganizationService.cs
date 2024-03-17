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

                var org = user.Organizations.Where(o => o.Id == request.OrganizationId)
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
               .FirstOrDefaultAsync(o => o.Id == request.OrganizationId);

                if (org == null)
                    return new GetOrganizationServiceResponse(
                        GetOrganizationServiceResponseStatus.OrganizationNotExists);

                return new GetOrganizationServiceResponse(
                       GetOrganizationServiceResponseStatus.Success)
                {
                    Name = org.Name,
                    Projects = org.Projects
                };
            }
            catch (Exception)
            {
                return new GetOrganizationServiceResponse(
                     GetOrganizationServiceResponseStatus.InternalError);
            }
        }

        public async Task<GetSubscribedOrganizationsServiceResponse> GetSubscribedOrganizations(GetSubscribedOrganizationsRequest request, string email)
        {
            try
            {
                var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);

                var userOrgs = await _context.Organizations
                    .Where(o => o.OwnerId == user.Id).ToListAsync();

                var memberOf = await _context.OrganizationEmployees
                    .Where(e => e.UserId == user.Id).ToListAsync();

                foreach (var e in memberOf)
                {
                    userOrgs.Add(await _context.Organizations
                        .Where(o => o.Id == e.OrganizationId).FirstOrDefaultAsync());
                }

                var response = new GetSubscribedOrganizationsServiceResponse(
                    GetSubscribedOrganizationsServiceResponseStatus.Success);

                foreach (var Org in userOrgs)
                {
                    response.Organizations.Add(new OrganizationForResponsteDto
                    {
                        Id = Org.Id,
                        Name = Org.Name,
                    });
                }

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
