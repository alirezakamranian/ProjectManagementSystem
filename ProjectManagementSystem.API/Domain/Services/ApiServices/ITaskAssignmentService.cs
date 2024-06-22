using Domain.Models.ApiModels.TaskAssignment.Request;
using Domain.Models.ServiceResponses.TaskAssignment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.ApiServices
{
    public interface ITaskAssignmentService
    {
        public Task<AssignTaskServiceResponse> AssignTask(AssignTaskRequest request,string userId);
        public Task<RemoveTaskAssignmentServiceResponse> RemoveAssignment(RemoveAssignmentRequest request,string userId);
    }
}
