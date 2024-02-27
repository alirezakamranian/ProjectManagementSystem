using Domain.Entities.HumanResource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Dtos.Employee;
namespace Domain.Services
{
    public interface IEmployeeService
    {
        public Task<IEnumerable<ResponseEmployeeDto>> GetAllEmployees();

        public Task CreateEmployee(CreateEmployeeDto employeeDto);

        public Task<ResponseEmployeeDto> GetEmployeeByID(int id);

        public Task UpdateEmployee(UpdateEmployeeDto employeeDto);

        public Task DeleteEmployeeById(int id);

    }
}
