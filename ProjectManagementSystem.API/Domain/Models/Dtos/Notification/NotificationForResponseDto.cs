using Domain.Constants.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Dtos.Notification
{
    public class NotificationForResponseDto
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public NotificationTypes Type { get; set; }

        public string Issuer { get; set; }
    }
}
