using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ApiModels.ProjectTask.Request
{
    public class DeleteProjectTaskRequest
    {
        [Required]
        public string TaskId { get; set; }
    }
}
