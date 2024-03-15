using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.Organization
{
    public class CreateOrganizationServiceResponse(string status)
    {
        public string Status { get; set; } = status;
    }
    public class CreateOrganizationServiceResponseStatus
    {
        public const string Success = "Success";
        public const string CreationFaild = "CreationFaild";
        public const string InternalError = "InternalError";
    }
}
