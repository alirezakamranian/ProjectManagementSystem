using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Models.Dtos.OrganizationInvitation.Response
{
    public class SearchUserResponse
    {
        public string Message { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<UserForResponseDto> Results { get; set; }
    }
    public class UserForResponseDto
    {
        public string Email { get; set; }
        public string Name { get; set; }
    }
}
