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
    public class TaskComment
    {
        [Key]
        public Guid Id { get; set; }
        public string Text { get; set; }

        //ProjectMemberRel
        public Guid MemberId { get; set; }
        [ForeignKey(nameof(MemberId))]
        public ProjectMember Member { get; set; }

        //ProjectTaskRel
        public Guid TaskId { get; set; }
        [ForeignKey(nameof (TaskId))]
        public ProjectTask Task { get; set; }

    }
}
