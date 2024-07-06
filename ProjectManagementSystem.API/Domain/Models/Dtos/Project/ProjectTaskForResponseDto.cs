using Domain.Entities.Project.ProjectTask;
using Domain.Models.Dtos.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Models.Dtos.Project
{
    public class ProjectTaskForResponseDto
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int Priority { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TaskAssignmentForResponseDto Assignment { get; set; }
        public TaskLabelForResponseDto Label { get; set; }
    }
}
