using Domain.Models.ServiceResponses.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.UserNotification
{
    public class DeleteNotificationServiceResponse(string status):ServiceResponseBase(status)
    {
    }
    public class DeleteNotificationServiceResponseStatus:ServiceResponseStatusBase
    {
    }
}
