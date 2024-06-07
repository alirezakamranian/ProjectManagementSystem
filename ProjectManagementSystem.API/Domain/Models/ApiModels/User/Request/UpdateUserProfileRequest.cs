using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ApiModels.User.Request
{
    public class UpdateUserProfileRequest
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string FullName { get; set; }
    }
}
