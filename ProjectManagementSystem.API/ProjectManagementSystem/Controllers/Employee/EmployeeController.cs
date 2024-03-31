using Domain.Models.Dtos.Auth.Response;
using Domain.Models.Dtos.Organization.Response;
using Domain.Models.Dtos.OrganizationEmployee.Request;
using Domain.Models.Dtos.OrganizationEmployee.response;
using Domain.Models.ServiceResponses.Organization;
using Domain.Models.ServiceResponses.OrganizationEmployee;
using Domain.Services.ApiServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProjectManagementSystem.Controllers.Employee
{
    [Route("organization/employee")]
    [ApiController]
    public class EmployeeController(IOrganizationEmployeeService employeeService) : ControllerBase
    {
        private readonly IOrganizationEmployeeService _employeeService = employeeService;

        [Authorize]
        [HttpPatch]
        public async Task<IActionResult> ChangeRole(ChangeEmployeeRoleRequest request)
        {
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest,
                       new ChangeEmployeeRoleResponse
                       {
                           Message = "DetailsAreRequired!"
                       });

            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id").Value;

            var serviceResponse =await _employeeService.ChangeEmployeeRole(request,userId);

            if (serviceResponse.Status == ChangeEmployeeRoleServiceResponseStatus.OrganizationNotExists ||
                serviceResponse.Status == ChangeEmployeeRoleServiceResponseStatus.AccessDenied ||
                serviceResponse.Status == ChangeEmployeeRoleServiceResponseStatus.EmployeeNotExists)
                return StatusCode(StatusCodes.Status400BadRequest,
                     new ChangeEmployeeRoleResponse
                     {
                         Message = serviceResponse.Status,
                     });

            if (serviceResponse.Status == ChangeEmployeeRoleServiceResponseStatus.InternalError)
                return StatusCode(StatusCodes.Status500InternalServerError,
                     new ChangeEmployeeRoleResponse
                     {
                         Message = serviceResponse.Status,
                     });

            return Ok(new ChangeEmployeeRoleResponse
            {
                Message = serviceResponse.Status
            });
        }
    }
}
