using Domain.Models.ServiceResponses.Base;
using Domain.Models.ServiceResponses.Organization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.ProjectTaskList
{
    public class ProjectTaskListServiceResponse(string status) : ServiceResponseBase(status)
    {
        public MinimumValueTaskListDto TaskList { get; set; }
}
    public class ProjectTaskListServiceResponseStatus : ServiceResponseStatusBase
    {
        public const string AccessDenied = "AccessDenied";
        public const string ProjectNotExists = "OrganizationNotExists";
    }
    public class MinimumValueTaskListDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
    }
}
