using Domain.Models.ApiModels.Project.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Dtos.Project
{
    public class ProjectTaskListForResponseDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int Priority { get; set; }

        public List<ProjectTaskForResponseDto> Tasks { get; set; }
    }
}
