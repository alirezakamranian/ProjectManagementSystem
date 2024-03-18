using Domain.Entities.HumanResource;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Common
{
    public class InvitationPending
    {
        [Key]
        public int Id { get; set; }

        public int OrganizationId { get; set; }

        public int NotificationId { get; set; }
    }
}
