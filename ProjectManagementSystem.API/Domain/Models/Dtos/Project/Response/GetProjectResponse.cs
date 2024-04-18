using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Dtos.Project.Response
{
    public class GetProjectResponse
    {
        public string Message { get; set; }
        public ProjectForResponseDto Project { get; set; }

    }

    public class ProjectForResponseDto
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime DeadLine { get; set; }

        public DateTime EndDate { get; set; }

        public string Status { get; set; }

        public List<ProjectTaskListForResponseDto> ProjectTaskLists { get; set; }
    }
    public class ProjectTaskListForResponseDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Priority { get; set; }

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