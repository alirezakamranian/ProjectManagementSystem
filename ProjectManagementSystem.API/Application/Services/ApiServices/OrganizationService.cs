using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.HumanResource;
using Domain.Models.Dtos.Organization.Request;
using Domain.Models.ServiceResponses.Organization;
using Domain.Services.ApiServices;
using Infrastructure.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace Application.Services.ApiServices
{
    public class OrganizationService(DataContext context) : IOrganizationService
    {
        private readonly DataContext _context = context;
        public async Task<CreateOrganizationServiceResponse> CreateOrganization(CreateOrganizationRequest request,string email)
        {
            try
            {
                var user = await _context.Users.Include(u=>u.Organizations).FirstOrDefaultAsync(u=>u.Email == email);

                user.Organizations.Add(new Organization
                {
                    Name = request.Name
                });
                await _context.SaveChangesAsync();

                return new CreateOrganizationServiceResponse(CreateOrganizationServiceResponseStatus.Success);
            }
            catch 
            { 
                return new CreateOrganizationServiceResponse(CreateOrganizationServiceResponseStatus.InternalError);
            }
           
        }
    }
}
