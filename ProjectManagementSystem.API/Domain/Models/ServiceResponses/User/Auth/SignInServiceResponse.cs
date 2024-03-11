using Domain.Models.Dtos.Auth.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ServiceResponses.User.Auth
{
    public class SignInServiceResponse
    {
        public string Message { get; set; }
        public SignInResponse UserData { get; set; }

        public SignInServiceResponse(string message)
        {
            Message = message.ToString();
        }
    }
}
