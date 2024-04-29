using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Dtos.ProjectTaskList.Request
{
    public class DeleteTaskListRequest
    {
        [Required]
        public string TaskListId { get; set; }
    }
}
