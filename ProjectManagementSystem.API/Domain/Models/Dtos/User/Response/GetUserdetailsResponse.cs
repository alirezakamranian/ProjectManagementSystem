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
        public string Status { get; set; }
        public List<NotificationForResponseDto> Notifications { get; set; }
        public string Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }
        
    }
    public class NotificationForResponseDto
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }

        public string Issuer { get; set; }
    }
}
