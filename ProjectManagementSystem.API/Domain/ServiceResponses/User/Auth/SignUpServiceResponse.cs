
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ServiceResponse.User.Auth
{
    public class SignUpServiceResponse
    {
        public string Message { get; set; }
        public SignUpServiceResponse(string message)
        {
            Message = message;
        }
    }
}
