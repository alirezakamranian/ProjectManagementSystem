using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ApiModels.ProjectMember.Request
{
    public class RemoveProjectMemberRequest
    {
        [Required]
        public string MemberId { get; set; }
        [Required]
        public string ProjectId { get; set; }
    }
}
