using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Dtos.Auth.Request
{
    public class RefreshTokenRequest(string email)
    {
        public string Email { get; set; } = email;
    }
}
