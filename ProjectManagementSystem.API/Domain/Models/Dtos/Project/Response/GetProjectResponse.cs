using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Models.Dtos.Project.Response
{
    public class GetProjectResponse
    {
        public string Message { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ProjectForResponseDto Project { get; set; }

    }

    public class ProjectForResponseDto
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        public List<ProjectTaskListForResponseDto> ProjectTaskLists { get; set; }
    }
    public class ProjectTaskListForResponseDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int Priority { get; set; }

        public List<ProjectTaskForResponseDto> Tasks { get; set; }
    }
    public class ProjectTaskForResponseDto
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int Priority { get; set; }
    }
}