using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos.Employee
{
    public class UpdateEmployeeDto
    {
        public string Name { get; set; }

        public string LastName { get; set; }


        public DateTime BirthDate { get; set; }


        public string Role { get; set; }


        public string Description { get; set; }
    }
}
