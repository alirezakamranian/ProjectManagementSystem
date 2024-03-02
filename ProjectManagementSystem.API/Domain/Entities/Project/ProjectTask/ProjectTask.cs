using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Project.ProjectTask
{
    public class ProjectTask
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string Title { get; set; }

        [MaxLength(700)]
        public string Description { get; set; }

        public string Priority { get; set; }

        //ProjectTaskListRel
        public int ProjectTaskListId { get; set; }
        [ForeignKey(nameof(ProjectTaskListId))]
        public ProjectTaskList ProjectTaskList { get; set; }

        //ProjTaskExecAgentRel
        public List<ProjectTaskExecutiveAgent> projectTaskExecutiveAgents { get; set; }
    }
}
