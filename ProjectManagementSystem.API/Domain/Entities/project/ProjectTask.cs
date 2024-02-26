using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Project
{
    public class ProjectTask
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public DateTime DeadLine { get; set; }


        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }



        public List<ProjectTaskExecutiveAgent> ProjectTaskExecutiveAgents { get; set; }
    }
}
