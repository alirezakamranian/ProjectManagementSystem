using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Dtos.Project
{
    public class ProjectForResponseDto
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        public List<ProjectTaskListForResponseDto> ProjectTaskLists { get; set; }
        public List<ProjectMemberForResponseDto> Members { get; set; }
    }
}
