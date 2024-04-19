using Domain.Models.Dtos.Project.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Dtos.ProjectTask.Response
{
    public class GetProjectTaskResponse
    {
        public string Message { get; set; }
        public ProjectTaskForResponseDto Task { get; set; }
    }
}
