using Domain.Entities.Common;
using Domain.Entities.HumanResource;
using Domain.Models.ApiModels.User.Response;
using Domain.Models.ApiModels.UserNotification.Response;
using Domain.Models.ServiceResponses.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.User
{
    public class GetUserDetailsServiceResponse(string status) : ServiceResponseBase(status)
    {
        public ApplicationUser User { get; set; }
        public string AvatarUrl { get; set; }
        public int NotificationsCount { get; set; }
    }
    public class GetUserDetailsServiceResponseStatus : ServiceResponseStatusBase
    {
    }
}
