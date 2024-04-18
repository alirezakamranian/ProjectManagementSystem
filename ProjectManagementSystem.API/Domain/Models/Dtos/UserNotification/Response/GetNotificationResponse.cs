using Domain.Constants.Notification;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Models.Dtos.UserNotification.Response
{
    public class GetNotificationResponse
    {
        public string  Message { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<NotificationForResponseDto> Notifications { get; set; }
    }
    public class NotificationForResponseDto
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public NotificationTypes Type { get; set; }

        public string Issuer { get; set; }
    }
}
