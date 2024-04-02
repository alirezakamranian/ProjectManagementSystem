using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Dtos.OrganizationInvitation.Request
{
    public class AcceptInvitationRequest
    {
        [Required]
        public string InviteId { get; set; }
    }
}
