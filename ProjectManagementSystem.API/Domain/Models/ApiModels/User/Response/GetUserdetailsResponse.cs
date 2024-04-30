using Domain.Entities.Common;
using Domain.Models.ApiModels.UserNotification.Response;
using Domain.Models.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Models.ApiModels.User.Response
{
    public class GetUserDetailsResponse
    {
        public string Message { get; set; }

        public int NotificationsCount { get; set; }

        public UserForResponseDto UserDetails { get; set; }

    }
}
