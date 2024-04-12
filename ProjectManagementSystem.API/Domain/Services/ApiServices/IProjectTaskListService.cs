using Domain.Models.Dtos.ProjectTaskList.Request;
using Domain.Models.ServiceResponses.ProjectTaskList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.ApiServices
{
    public interface IProjectTaskListService
    {
        public Task<ProjectTaskListServiceResponse> CreateTaskList(CreateTaskListRequest request);
    }
}
