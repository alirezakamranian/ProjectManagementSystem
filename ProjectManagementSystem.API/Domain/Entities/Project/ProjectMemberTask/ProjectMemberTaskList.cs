using Domain.Entities.HumanResource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Project.ProjectMemberTask
{
    public class ProjectMemberTaskList
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [Required]
        public string Order { get; set; }

        //ProjMemberRel
        public int ProjectMemberId { get; set; }
        [ForeignKey(nameof(ProjectMemberId))]
        public ProjectMember ProjectMember{ get; set; }

        //ProjMemberTaskRel
        public List<Entities.Project.ProjectMemberTask.ProjectMemberTask> ProjectMemberTasks { get; set; }
    }
}
