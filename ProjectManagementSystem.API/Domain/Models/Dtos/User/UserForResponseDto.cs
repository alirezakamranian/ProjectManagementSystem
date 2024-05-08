using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Dtos.User
{
    public class UserForResponseDto
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }

        public string AvatarUrl { get; set; }
        public string Name { get; set; }
    }
}
