using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ApiModels.OrganizationInvitation.Request
{
    public class InviteEmployeeRequest
    {
        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }
        [Required]
        public string OrganizationId { get; set; }
        public string Message { get; set; }
    }
}
