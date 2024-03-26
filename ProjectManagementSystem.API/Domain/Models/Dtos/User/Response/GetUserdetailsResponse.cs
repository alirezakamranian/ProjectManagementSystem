using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Dtos.User.Response
{
    public class GetUserDetailsResponse
    {
        public List<NotificationForResponseDto> Notifications { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
    }
}
