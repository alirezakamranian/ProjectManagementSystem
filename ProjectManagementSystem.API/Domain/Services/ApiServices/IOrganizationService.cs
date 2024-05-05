using Domain.Entities.HumanResource;
using Domain.Models.ApiModels.Organization.Request;
using Domain.Models.ApiModels.Organization.Response;
using Domain.Models.ServiceResponses.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.ApiServices
{
    public interface IOrganizationService
    {
        public Task<CreateOrganizationServiceResponse> CreateOrganization(CreateOrganizationRequest request, string userId);
        public Task<UpdateOrganizationServiceResponse> UpdateOrganization(UpdateOrganizationRequest request, string userId);
        public Task<GetSubscribedOrganizationsServiceResponse> GetSubscribedOrganizations(string userId);
        public Task<GetOrganizationServiceResponse> GetOrganization(GetOrganizationRequest request);
        public Task<RemoveOrganizationServiceResponse> RemoveOrganization(RemoveOrganizationRequest request,string userId);   
    }
}
