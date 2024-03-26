using Domain.Entities.Common;
using Domain.Models.Dtos.User.Response;
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
    }
    public class GetUserDetailsServiceResponseStatus:ServiceResponseStatusBase
    {
    }
}
