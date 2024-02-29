using Domain.Entities.HumanResource;
using Domain.Entities.Project.ProjectTask;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Project
{
    public class Project
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [MaxLength(700)]
        public string Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime DeadLine { get; set; }

        public DateTime EndDate { get; set; }

        public string Status { get; set; }
 
        //OrgRel
        public int OrganizationId { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        public Organization Organization{ get; set; }

        //PorjTaskRel
        public List<ProjectTaskList> ProjectTaskLists { get; set; }

        //ProjMemberRel
        public List<ProjectMember> ProjectMembers{ get; set; }
    }
}
