using Domain.Entities.Common;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.HumanResource
{
    public class ApplicationUser:IdentityUser
    {
        public string FullName { get; set; }

        //OrgRel
        public List<Organization> Organizations { get; set; }

        //OrgEmployeeRel
        public List<OrganizationEmployee> OrganizationEmployees { get; set; }

        //NotifRel
        public List<Notification> Notifications { get; set; }

    }
}
