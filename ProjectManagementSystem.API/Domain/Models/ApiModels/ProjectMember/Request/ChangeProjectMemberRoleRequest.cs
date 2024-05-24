using Domain.Constants.Roles.OrganiationEmployees;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ApiModels.ProjectMember.Request
{
    public class ChangeProjectMemberRoleRequest
    {
        [Required]
        public string ProjectId { get; set; }
        [Required]
        public string MemberId { get; set; }
        [Required]
        public ProjectMemberRoles NewRole { get; set; }
    }
}
