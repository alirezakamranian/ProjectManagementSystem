using Domain.Models.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Models.ApiModels.OrganizationInvitation.Response
{
    public class SearchUserResponse
    {
        public string Message { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<MinimalValueUserForResponseDto> Results { get; set; }
    }
   
}
