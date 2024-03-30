using Domain.Entities.HumanResource;
using Domain.Models.ServiceResponses.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.OrganizationInvitation
{
    public class SearchUserServiceResponse(string status):ServiceResponseBase(status)
    {
        public List<ApplicationUser> Users { get; set; }
    }
    public class SearchUserServiceResponseStatus:ServiceResponseStatusBase
    {

    }
}
