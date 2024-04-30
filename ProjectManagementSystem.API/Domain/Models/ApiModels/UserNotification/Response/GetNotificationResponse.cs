using Domain.Constants.Notification;
using Domain.Models.Dtos.Notification;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Models.ApiModels.UserNotification.Response
{
    public class GetNotificationResponse
    {
        public string Message { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<NotificationForResponseDto> Notifications { get; set; }
    }
}
