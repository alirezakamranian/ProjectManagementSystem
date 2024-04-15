using Domain.Constants.Roles.OrganiationEmployees;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
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
        //[JsonConverter(typeof(JsonStringEnumConverter))]
        public OrganizationEmployeesRoles Role { get; set; }
    }
}
