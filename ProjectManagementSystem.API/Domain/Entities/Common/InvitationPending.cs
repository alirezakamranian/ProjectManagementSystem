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
        public Guid Id { get; set; }

        public Guid OrganizationId { get; set; }

        public Guid NotificationId { get; set; }
    }
}
