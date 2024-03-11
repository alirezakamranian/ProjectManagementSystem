using Domain.Entities.Project.ProjectMemberTask;
using Domain.Entities.Project.ProjectTask;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.HumanResource
{
    public class ProjectMember
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string Role { get; set; }


        //ProjectRel
        public int ProjectId { get; set; }

        [ForeignKey("ProjectId")]
        public Project.Project Project { get; set; }

        //OrgEmployeeRel
        public int OrganizationEmployeeId { get; set; }

        [ForeignKey("OrganizationEmployeeId")]
        public OrganizationEmployee OrganizationEmployee { get; set; }


        //ProjMemberTaskListRel
        public List<ProjectMemberTaskList> MemberTaskLists { get; set; }

        //ProjTaskExecAgetRel
        public List<ProjectTaskExecutiveAgent> projectTaskExecutiveAgents{ get; set; }
    }
}
