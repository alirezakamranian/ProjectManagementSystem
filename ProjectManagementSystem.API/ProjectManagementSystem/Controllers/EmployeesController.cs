using Application.Services.EmployeeService;
using Domain.Entities.HumanResource;
using Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain.Dtos.Employee;
using Microsoft.EntityFrameworkCore;

namespace ProjectManagementSystem.Controllers
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private IEmployeeService _employeeService;
        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]   
        public async Task<ActionResult<IEnumerable<ResponseEmployeeDto>>> Get()
        {
            try
            {
                return Ok(await _employeeService.GetAllEmployees());
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseEmployeeDto>> Get(int id)
        {
            try
            {
                var response = await _employeeService.GetEmployeeByID(id);

                if (response == null)
                    return NotFound();

                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateEmployeeDto employee)
        {

            try
            {
                await _employeeService.CreateEmployee(employee);

                return CreatedAtAction("Get", employee);
            }
            catch
            {
                return StatusCode(500, "Internal Server Error");
            }

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateEmployeeDto employee)
        {
            try
            {
                await _employeeService.UpdateEmployee(employee);

                return Ok("Updated");
            }
            catch
            {
                return StatusCode(500, "Internal Server Error");
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
               await _employeeService.DeleteEmployeeById(id);
                return Ok("Deleted");
            }
            catch
            {
                return StatusCode(500, "Internal Server Error");
            }

        }
    }
}
