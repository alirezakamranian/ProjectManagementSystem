using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Dtos.ProjectMember.Request
{
    public class AddMemberRequest
    {
        [Required]
        public string EmployeeId { get; set; }
        [Required]
        public string ProjectId { get; set; }
    }
}
