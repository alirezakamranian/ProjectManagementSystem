using Domain.Constants.Roles.OrganiationEmployees;
using System.Text.Json.Serialization;
using Domain.Models.Dtos.Project;
namespace Domain.Models.ApiModels.Project.Response
{
    public class GetProjectResponse
    {
        public string Message { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ProjectForResponseDto Project { get; set; }

    }
}