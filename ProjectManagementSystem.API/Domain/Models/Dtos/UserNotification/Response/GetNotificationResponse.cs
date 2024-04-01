using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Dtos.UserNotification.Response
{
    public class GetNotificationResponse
    {
        public string  Message { get; set; }
        public List<NotificationForResponseDto> Notificatins { get; set; }
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
