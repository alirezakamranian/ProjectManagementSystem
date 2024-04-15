using Domain.Constants.Roles.OrganiationEmployees;
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
        public Guid Id { get; set; }

        [MaxLength(50)]
        [Required]
        public ProjectMemberRoles Role { get; set; }

        //ProjectRel
        public Guid ProjectId { get; set; }

        [ForeignKey("ProjectId")]
        public Project.Project Project { get; set; }

        //OrgEmployeeRel
        public Guid OrganizationEmployeeId { get; set; }

        [ForeignKey("OrganizationEmployeeId")]
        public OrganizationEmployee OrganizationEmployee { get; set; }
    }
}
