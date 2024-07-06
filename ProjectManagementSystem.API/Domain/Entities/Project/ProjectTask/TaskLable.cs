using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Project.ProjectTask
{
    public class TaskLable
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        [Required]
        public string Color { get; set; }

        //ProjectRel
        public Guid ProjectId { get; set; }
        [ForeignKey(nameof(ProjectId))]
        public Project Project { get; set; }

        //TaskLabelAttachmentRel
        public List<TaskLabelAttachment> LabelAttachments { get; set; }
    }
}
