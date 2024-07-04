using Domain.Models.ApiModels.TaskComment.Request;
using Domain.Models.ServiceResponses.TaskComment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.ApiServices
{
    public interface ITaskCommentService
    {
        public Task<AddTaskCommentServiceResponse> AddComment(AddTaskCommentRequest request, string userId);
        public Task<GetTaskCommentServiceResponse> GetTaskComments(GetTaskCommentsRequest request, string userId);
        public Task<RemoveTaskCommentServiceResponse> RemoveTaskComment(RemoveTaskCommentRequest request, string userId);
    }
}
