using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Dtos.OrganizationInvitation.Request
{
    public class InviteEmployeeRequest
    {
        public string UserEmail { get; set; }
        public string Message { get; set; }
    }
}
