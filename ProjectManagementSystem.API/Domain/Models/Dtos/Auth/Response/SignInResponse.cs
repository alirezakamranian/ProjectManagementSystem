using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Dtos.Auth.Response
{
    public class SignInResponse
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
