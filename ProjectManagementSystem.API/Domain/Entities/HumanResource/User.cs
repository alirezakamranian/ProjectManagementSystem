using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.HumanResource
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [MaxLength(50)]
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        //OrgRel
        public List<Organization> Organizations { get; set; }

        //OrgEmployeeRel
        public List<OrganizationEmployee> OrganizationEmployees { get; set; }

        //NotifRel
        public List<Notification> Notifications { get; set; }

    }
}
