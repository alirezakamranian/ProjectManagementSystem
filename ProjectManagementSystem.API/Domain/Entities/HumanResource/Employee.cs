using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Project;
using Domain.Entities.HumanResource.Team;

namespace Domain.Entities.HumanResource
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [MaxLength(50)]
        [Required]
        public string LastName { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public DateTime RecruitmentDate { get; set; }

        [MaxLength]
        [Required]
        public string Role { get; set; }

        [MaxLength(300)]
        public string Description { get; set; }

        public ProjectTaskExecutiveAgent ProjectTaskExecutiveAgent { get; set; }

        public List<TeamMember> TeamMembers { get; set; }

        public List<ProjectManagement> ProjectManagement { get; set; }
    }
}
