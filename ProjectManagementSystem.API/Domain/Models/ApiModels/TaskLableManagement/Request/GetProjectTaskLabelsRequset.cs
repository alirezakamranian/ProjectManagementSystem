using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ApiModels.TaskLableManagement.Request
{
    public class GetProjectTaskLabelsRequset
    {
        [Required]
        public string ProjectId { get; set; }
    }
}
