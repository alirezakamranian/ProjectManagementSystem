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
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string Role { get; set; }

        //UserRel
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        //OrgRel
        public int OrganizationId { get; set; }

        [ForeignKey("OrganizationId")]
        public Organization Organization { get; set; }

        //  ProjMemberRel
        public List<ProjectMember> projectMembers { get; set; }
    }
}
