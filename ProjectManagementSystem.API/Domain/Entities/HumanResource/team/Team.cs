using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Project;
namespace Domain.Entities.HumanResource.Team
{
    public class Team
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        [Required]
        public string Name { get; set; }


        public List<TeamMember> TeamMembers { get; set; }

        public ProjectTaskExecutiveAgent ProjectTaskExecutiveAgent { get; set; }
    }
}
