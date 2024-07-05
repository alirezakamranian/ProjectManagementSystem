using Domain.Models.ApiModels.TaskLableManagement.Request;
using Domain.Models.ServiceResponses.TaskLabelManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.ApiServices
{
    public interface ITaskLableManagementService
    {
        public Task<CreateTaskLabelServiceResponse> CreateTaskLabel(CreateTaskLabelRequest request, string userId);
    }
}
