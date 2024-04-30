using Domain.Models.ApiModels.Project.Request;
using Domain.Models.ServiceResponses.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.ApiServices
{
    public interface IProjectService
    {
        public Task<CreateProjectServiceResponse> CreateProject(CreateProjectRequest request, string email);
        public Task<GetProjectServiceResponse> GetProject(GetProjectRequest request, string userId);
        public Task<DeleteProjectServiceResponse> DeleteProject(DeleteProjectRequest request, string userId);
        public Task<ChangeProjectLeaderServiceResponse> ChangeLeadr(ChangeProjectLeaderRequest request, string userId);
    }
}
