using Domain.Constants.Roles.OrganiationEmployees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Dtos.Project
{
    public class ProjectMemberForResponseDto
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public ProjectMemberRoles Role { get; set; }
        public string Name { get; set; }
        public string AvatarUrl { get; set; }
    }
}
