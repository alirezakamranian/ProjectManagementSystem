using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ApiModels.TaskLabelAttachment.Request
{
    public class AttachTaskLabelRequest
    {
        [Required]
        public string TaskId { get; set; }
        [Required]
        public string LabelId { get; set; }
    }
}
