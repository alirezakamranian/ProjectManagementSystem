using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Dtos.Auth.Response
{
    public class RefreshTokenResponse
    {
        public string Message { get; set; }
        public string Token { get; set; }
    }
}
