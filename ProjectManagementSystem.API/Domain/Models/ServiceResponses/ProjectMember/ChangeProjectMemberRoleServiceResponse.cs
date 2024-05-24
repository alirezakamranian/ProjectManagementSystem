using Domain.Models.ServiceResponses.Base;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.ProjectMember
{
    public class ChangeProjectMemberRoleServiceResponse(string status) : ServiceResponseBase(status)
    {
    }
    public class ChangeProjectMemberRoleServiceResponseStatus : ServiceResponseStatusBase
    {
        public const string MemberNotExists = "MemberNotExsits";
        public const string ProjectNotExists = "ProjectNotExists";
        public const string AccessDenied = "AccessDenied";
        public const string LeaderRoleCanNotChanged = "LeaderRoleCanNotChanged";
    }
}