using Domain.Entities.HumanResource;
using Domain.Services;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Dtos.Employee;
namespace Application.Services.EmployeeService
{
    public class EmployeeService : IEmployeeService
    {
        private DataContext _context;
        public EmployeeService(DataContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<ResponseEmployeeDto>> GetAllEmployees()
        {
            var employees = await _context.Employees.ToListAsync();
            var response = new List<ResponseEmployeeDto>();

            foreach (var employee in employees)
            {
                response.Add(new ResponseEmployeeDto()
                {
                    Name = employee.Name,
                    LastName = employee.LastName,
                    Role = employee.Role,
                    BirthDate = employee.BirthDate,
                    RecruitmentDate = employee.RecruitmentDate,
                    Description = employee.Description
                });
            }
            return response;
        }

        public async Task<ResponseEmployeeDto> GetEmployeeByID(int id)
        {
            var employee = await _context.Employees.FirstAsync(x => x.Id == id);

            return new ResponseEmployeeDto()
            {
                Name = employee.Name,
                LastName = employee.LastName,
                Role = employee.Role,
                BirthDate = employee.BirthDate,
                RecruitmentDate = employee.RecruitmentDate,
                Description = employee.Description
            };
        }

        public async Task CreateEmployee(CreateEmployeeDto employeeDto)
        {
            await _context.Employees.AddAsync(new Employee()
            {
                Name = employeeDto.Name,
                LastName = employeeDto.LastName,
                BirthDate = employeeDto.BirthDate,
                RecruitmentDate = DateTime.Now.Date,
                Role = employeeDto.Role,
                Description = employeeDto.Description
            });

            await _context.SaveChangesAsync();
        }

        public async Task DeleteEmployeeById(int id)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
    
                _context.Employees.Remove(employee);

                await _context.SaveChangesAsync();
        }
        public async Task UpdateEmployee(UpdateEmployeeDto employeeDto)
        {
            _context.Employees.Update(new Employee()
            {
                Name = employeeDto.Name,
                LastName = employeeDto.LastName,
                Role = employeeDto.Role,
                BirthDate = employeeDto.BirthDate,
                Description = employeeDto.Description
            });

            await _context.SaveChangesAsync();
        }
    }
}