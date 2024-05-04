using Domain.Models.ServiceResponses.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.ProjectMember
{
    public class RemoveProjectMemberServiceResponse(string status) : ServiceResponseBase(status)
    {
    }
    public class RemoveProjectMemberServiceResponseStatus : ServiceResponseStatusBase
    {
        public const string AccessDenied = "AccessDenied";
        public const string MemberNotExists = "MemberNotExists";
        public const string ProjectNotExists = "ProjectNotExists";
        public const string LeaderCanNotRemoved = "LeaderCanNotRemoved";
    }
}
