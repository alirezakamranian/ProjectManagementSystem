using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.Base
{
    public class ServiceResponseBase(string status)
    {
        public string Status { get; set; } = status;
    }
    public class ServiceResponseStatusBase
    {
        public const string Success = "Success";
        public const string InternalError = "InternalError";
    }
}
