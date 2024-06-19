using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Dtos.Project
{
    public class TaskAssignmentForResponseDto
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string MemberId { get; set; }
    }
}
