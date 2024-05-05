using Domain.Models.ServiceResponses.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.Storage
{
    public class GetFileUrlServiceResponse(string status):ServiceResponseBase(status)
    {
        public string Url { get; set; }
    }
    public class GetFileUrlServiceResponseStatus:ServiceResponseStatusBase
    {
    }
}
