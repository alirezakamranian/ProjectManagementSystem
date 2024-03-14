using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Dtos.Auth.Request
{
    public class SignUpRequest
    {
        public string FullName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}
