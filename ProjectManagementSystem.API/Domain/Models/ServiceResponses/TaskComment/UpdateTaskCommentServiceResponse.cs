using Domain.Models.ServiceResponses.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.TaskComment
{
    public class UpdateTaskCommentServiceResponse(string status):ServiceResponseBase(status)
    {
    }
    public class UpdateTaskCommentServiceResponseStatus:ServiceResponseStatusBase
    {
        public const string AccessDenied = "AccessDenied";
        public const string CommentNotExists = "CommentNotExists";
    }
}
