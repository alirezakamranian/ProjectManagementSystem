using Domain.Entities.Common;
using Domain.Entities.HumanResource;
using Domain.Models.Dtos.User.Response;
using Domain.Models.Dtos.UserNotification.Response;
using Domain.Models.ServiceResponses.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.User
{
    public class GetUserDetailsServiceResponse(string status):ServiceResponseBase(status)
    {
        public List<NotificationForResponseDto> Notifications { get; set; }
        public ApplicationUser User { get; set; }
    }
    public class GetUserDetailsServiceResponseStatus:ServiceResponseStatusBase
    {
    }
}
