using Domain.Models.ApiModels.Project.Response;
using Domain.Models.Dtos.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Models.ApiModels.ProjectTask.Response
{
    public class CreateProjectTaskResponse
    {
        public string Message { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ProjectTaskForResponseDto NewTask { get; set; }
    }
}
