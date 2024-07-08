using Domain.Models.Dtos.Task;
using Domain.Models.ServiceResponses.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.TaskLabelManagement
{
    public class GetProjectTaskLabelsServiceResponse(string status):ServiceResponseBase(status)
    {
        public List<TaskLabelForResponseDto> Labels { get; set; }
    }
    public class GetProjectTaskLabelsServiceResponseStatus:ServiceResponseStatusBase
    {
        public const string AccessDenied = "AccessDenied";
        public const string ProjectNotExists = "ProjectNotExists";
    }
}
