using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ApiModels.TaskComment.Request
{
    public class UpdateTaskCommentRequest
    {
        [Required]
        public string CommentId { get; set; }
        [Required]
        public string NewContent { get; set; }
    }
}
