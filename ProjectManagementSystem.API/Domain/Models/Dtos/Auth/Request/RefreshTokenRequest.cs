using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Dtos.Auth.Request
{
    public class RefreshTokenRequest
    {
        [Required]
        public string RefreshToken { get; set; } 
    }
}
