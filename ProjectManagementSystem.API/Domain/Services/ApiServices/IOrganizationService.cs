using Domain.Models.Dtos.Organization.Request;
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
        public Task<CreateOrganizationServiceResponse> CreateOrganization(CreateOrganizationRequest request,string email);
    }
}
