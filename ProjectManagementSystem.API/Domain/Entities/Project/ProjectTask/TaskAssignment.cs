using Domain.Entities.HumanResource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Project.ProjectTask
{
    public class TaskAssignment
    {
        [Key]
        public Guid Id { get; set; }

        public string Description { get; set; }

        public Guid ProjectMemberId { get; set; }
        [ForeignKey(nameof(ProjectMemberId))]
        public ProjectMember Member { get; set; }

        public Guid ProjectTaskId { get; set; }
        [ForeignKey(nameof(ProjectTaskId))]
        public ProjectTask projectTask { get; set; }
    }
}
