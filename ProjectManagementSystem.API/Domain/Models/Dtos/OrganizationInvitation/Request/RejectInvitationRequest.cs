using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Dtos.OrganizationInvitation.Request
{
    public class RejectInvitationRequest
    {
        [Required]
        public string InviteId { get; set; }
    }
}
