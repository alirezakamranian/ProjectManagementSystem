using Domain.Models.ServiceResponses.Base;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.Project
{
    public class DeleteProjectServiceResponse(string status) : ServiceResponseBase(status)
    {
    }
    public class DeleteProjectServiceResponseStatus : ServiceResponseStatusBase
    {
        public const string ProjectNotExists = "ProjectNotExists";
        public const string AccessDenied = "AccessDenied";
    }
}
