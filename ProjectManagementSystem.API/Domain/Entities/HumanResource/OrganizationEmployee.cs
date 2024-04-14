using Domain.Constants.Roles.OrganiationEmployees;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.HumanResource
{
    public class OrganizationEmployee
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(50)]
        [Required]
        public OrganizationEmployeesRoles Role { get; set; }

        //UserRel
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        //OrgRel
        public Guid OrganizationId { get; set; }

        [ForeignKey("OrganizationId")]
        public Organization Organization { get; set; }

        //  ProjMemberRel
        public List<ProjectMember> projectMembers { get; set; }
    }
}
