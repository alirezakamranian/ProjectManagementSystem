using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Project
{
    public class Project
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        [Required]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime DeadLine { get; set; }

        public string Status { get; set; }


        public List<ProjectTask> ProjectTasks { get; set; }

        public ProjectManagement ProjectManagement { get; set; }
    }
}
