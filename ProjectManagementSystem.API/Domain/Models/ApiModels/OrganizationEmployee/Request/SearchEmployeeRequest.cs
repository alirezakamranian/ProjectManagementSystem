using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ApiModels.OrganizationEmployee.Request
{
    public class SearchEmployeeRequest
    {
        public string Query { get; set; }
        public string OrganizationId{ get; set; }
    }
}
