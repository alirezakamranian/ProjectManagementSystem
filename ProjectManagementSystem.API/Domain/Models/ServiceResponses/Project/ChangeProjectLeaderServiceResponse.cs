using Domain.Models.ServiceResponses.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.Project
{
    public class ChangeProjectLeaderServiceResponse(string status):ServiceResponseBase(status)
    {
    }
    public class ChangeProjectLeaderServiceResponseStatus:ServiceResponseStatusBase
    {
        public const string AccessDenied = "AccessDenied";
        public const string LeaderNotExists = "LeaderNotExists";
        public const string ProjectNotExists = "ProjectNotExists";
    }
}
