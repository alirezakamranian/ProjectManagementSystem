using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Project.ProjectTask
{
    public class TaskLabelAttachment
    {
        [Key]
        public Guid Id { get; set; }

        public Guid TaskId { get; set; }
        [ForeignKey(nameof(TaskId))]
        public ProjectTask Task { get; set; }

        public Guid LabelId { get; set; }
        [ForeignKey(nameof(LabelId))]
        public TaskLable Label { get; set; }
    }
}
