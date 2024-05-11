using Domain.Entities.Project;
using Domain.Models.Dtos.Project;
using Domain.Models.ServiceResponses.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.Project
{
    public class GetProjectServiceResponse(string status) : ServiceResponseBase(status)
    {
        public ProjectForResponseDto Project { get; set; }
        public string AvatarUrl { get; set; }
    }
    public class GetProjectServiceResponseStatus : ServiceResponseStatusBase
    {
        public const string ProjectNotExists = "ProjectNotExists"; 

        public const string AccessDenied = "AccessDenied";
    }
}
