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
    public class ProjectTaskExecutiveAgent
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string Role { get; set; }


        //ProjectTaskRel
        public int ProjectTaskId { get; set; }

        [ForeignKey(nameof(ProjectTaskId))]
        public ProjectTask ProjectTask { get; set; }

        //ProjMemberRel
        public int ProjectMemberId { get; set; }

        [ForeignKey("ProjectMemberId")]
        public ProjectMember ProjectMember { get; set; }
    }
}
