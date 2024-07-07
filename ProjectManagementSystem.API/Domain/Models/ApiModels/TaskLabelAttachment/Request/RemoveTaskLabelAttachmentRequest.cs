using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ApiModels.TaskLabelAttachment.Request
{
    public class RemoveTaskLabelAttachmentRequest
    {
        [Required]
        public string TaskId { get; set; }
    }
}
