using Domain.Models.ApiModels.ProjectMember.Request;
using Domain.Models.ServiceResponses.ProjectMember;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.ApiServices
{
    public interface IProjectMemberService
    {
        public Task<AddMemberServiceResponse> AddMember(AddMemberRequest request, string userId);
    }
}
