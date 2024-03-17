using Domain.Models.Dtos.OrganizationInvitation.Request;
using Domain.Models.ServiceResponses.OrganizationInvitation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.ApiServices
{
    public interface IOrganizationInvitationService
    {
        public Task<InviteEmployeeServiceResponse> InviteEmployee(InviteEmployeeRequest request, string issuerEmail);
        public Task<AcceptOrganizationInvitationServiceResponse> AcceptOrganizationInvitation(AcceptInvitationRequest request,string email);
        public Task<RejectOrganizationInvitationServiceResponse> RejectOrganizationInvitation(RejectInvitationRequest request, string email);
    }
}
