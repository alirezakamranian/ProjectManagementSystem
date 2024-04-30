using Domain.Models.ApiModels.ProjectTask.Request;
using Domain.Models.ServiceResponses.ProjectTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.ApiServices
{
    public interface IProjectTaskService
    {
        public Task<CreateProjectTaskServiceResponse> CreateTask(CreateProjectTaskRequest request, string userId);
        public Task<GetProjectTaskServiceResponse> GetTask(GetProjectTaskRequest request, string userId);
        public Task<DeleteProjectTaskServiceResponse> DeleteTask(DeleteProjectTaskRequest request, string userId);
        public Task<ChangeProjectTaskPriorityServiceResponse> ChangePriority(ChangeProjectTaskPriorityRequest request,string userId);
        public Task<UpdateProjectTaskServiceResponse> UpdateTask(UpdateProjectTaskRequest request, string userId);
        public Task<ChangeProjectTasksTaskListServiceResponse> ChangeTaskList(ChangeProjectTasksTaskListRequest request,string userId);
    }
}
