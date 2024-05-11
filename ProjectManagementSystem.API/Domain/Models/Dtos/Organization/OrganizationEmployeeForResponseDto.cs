using Domain.Constants.Roles.OrganiationEmployees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Dtos.Organization
{
    public class OrganizationEmployeeForResponseDto
    {
        public string Id { get; set; }
        public OrganizationEmployeesRoles Role { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string AvatarUrl { get; set; }
    }
}
