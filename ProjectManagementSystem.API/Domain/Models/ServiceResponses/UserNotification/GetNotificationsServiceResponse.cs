using Domain.Entities.Common;
using Domain.Models.ServiceResponses.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.UserNotification
{
    public class GetNotificationsServiceResponse(string status):ServiceResponseBase(status)
    {
        public List<Notification> Notifications { get; set; }
    }
    public class GetNotificationsServiceResponseStatus:ServiceResponseStatusBase
    {
    }
}
