using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ApiModels.TaskLableManagement.Request
{
    public class CreateTaskLabelRequest
    {
        [Required]
        public string ProjectId { get; set; }
        public string Title { get; set; }
        [Required]
        public string Color { get; set; }
    }
}
