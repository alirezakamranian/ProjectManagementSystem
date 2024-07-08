using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ApiModels.TaskLableManagement.Request
{
    public class RemoveTaskLabelRequest
    {
        [Required]
        public string LabelId { get; set; }
    }
}
