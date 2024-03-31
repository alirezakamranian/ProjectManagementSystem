using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Dtos.OrganizationEmployee.Request
{
    public class ChangeEmployeeRoleRequest
    {
        [Required]
        public string OrganizationId { get; set; }
        [Required]
        public string EmployeeId { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
