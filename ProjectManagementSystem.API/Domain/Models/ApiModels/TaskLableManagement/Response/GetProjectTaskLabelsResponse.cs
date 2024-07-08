using Domain.Models.Dtos.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ApiModels.TaskLableManagement.Response
{
    public class GetProjectTaskLabelsResponse
    {
        public string Message { get; set; }
        public List<TaskLabelForResponseDto> Labels { get; set; }
    }
}
