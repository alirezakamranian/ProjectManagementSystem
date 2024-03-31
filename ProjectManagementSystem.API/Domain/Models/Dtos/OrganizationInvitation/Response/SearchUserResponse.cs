using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Dtos.OrganizationInvitation.Response
{
    public class SearchUserResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public List<UserForResponseDto> Results { get; set; }
    }
    public class UserForResponseDto
    {
        public string Email { get; set; }
        public string Name { get; set; }
    }
}
