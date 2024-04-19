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
        public Guid Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [MaxLength(700)]
        public string Description { get; set; }

        public string Status { get; set; }

        public string LeaderId { get; set; }
        
        //OrgRel
        public Guid OrganizationId { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        public Organization Organization { get; set; }

        //PorjTaskRel
        public List<ProjectTaskList> ProjectTaskLists { get; set; }

        //ProjMemberRel
        public List<ProjectMember> ProjectMembers { get; set; }

        public string Creationlevel { get; set; }
    }
}
