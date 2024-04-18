using Domain.Entities.Common;
using Domain.Models.Dtos.UserNotification.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Models.Dtos.User.Response
{
    public class GetUserDetailsResponse
    {
        public string Message { get; set; }

        public int NotificationsCount { get; set; }

        public UserForResponseDto UserDetails { get; set; }

    }
    public class UserForResponseDto
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }

        public string Avatar { get; set; }
    }
}
