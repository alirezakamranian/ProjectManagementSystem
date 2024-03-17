using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Dtos.Auth.Response
{
    public class SignInResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public string RefrshToken { get; set; }
    }
}
