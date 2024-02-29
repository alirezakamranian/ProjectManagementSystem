using Domain.Entities.Project;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
namespace Domain.Entities.HumanResource
{
    public class Organization
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; }


        //ProjectRel
        public List<Project.Project> Projects { get; set; }

        //OrgMemberRel
        public List<OrganizationEmployee> OrganizationEmployees { get; set; }

        //UserRel
        public int OwnerId { get; set; }
        [ForeignKey(nameof(OwnerId))]
        public User Owner { get; set; }
    }
}
