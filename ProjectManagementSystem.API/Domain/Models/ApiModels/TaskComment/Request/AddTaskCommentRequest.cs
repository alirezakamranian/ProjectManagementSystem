using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ApiModels.TaskComment.Request
{
    public class AddTaskCommentRequest
    {
        [Required] 
        public string TaskId { get; set; }
        public string Text { get; set; } 
    }
}
