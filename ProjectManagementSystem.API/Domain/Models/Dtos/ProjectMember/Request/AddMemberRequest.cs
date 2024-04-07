using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Dtos.ProjectMember.Request
{
    public class AddMemberRequest
    {
        public string EmployeeId { get; set; }
        public string ProjectId { get; set; }
    }
}
