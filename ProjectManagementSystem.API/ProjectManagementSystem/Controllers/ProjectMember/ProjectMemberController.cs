using Domain.Models.ApiModels.ProjectMember.Request;
using Domain.Models.ApiModels.Organization.Response;
using Domain.Models.ApiModels.ProjectMember.Response;
using Domain.Models.ServiceResponses.Organization;
using Domain.Models.ServiceResponses.ProjectMember;
using Domain.Services.ApiServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Domain.Models.ApiModels.OrganizationEmployee.response;

namespace ProjectManagementSystem.Controllers.ProjectMember
{
    /// <summary>
    /// Provides ProjectMemberService using REST-ful services
    /// </summary>
    /// <param name="projectMemberService"></param>
    [Route("organization/project/member")]
    [ApiController]
    public class ProjectMemberController(IProjectMemberService projectMemberService) : ControllerBase
    {
        private readonly IProjectMemberService _projectMemberService = projectMemberService;

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddMember(AddMemberRequest request)
        {
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest,
                        new AddMemberResponse
                        {
                            Message = "DetailsAreInvalid"
                        });

            var userId = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type.Equals("Id")).Value;

            var serviceResponse = await _projectMemberService
                .AddMember(request, userId);

            if (serviceResponse.Status.Equals(AddMemberServiceResponseStatus.ProjectNotExists) ||
                serviceResponse.Status.Equals(AddMemberServiceResponseStatus.EmployeeNotExists) ||
                serviceResponse.Status.Equals(AddMemberServiceResponseStatus.AccessDenied))
                return StatusCode(StatusCodes.Status400BadRequest,
                        new AddMemberResponse
                        {
                            Message = serviceResponse.Status
                        });

            if (serviceResponse.Status.Equals(AddMemberServiceResponseStatus.InternalError))
                return StatusCode(StatusCodes.Status500InternalServerError,
                        new AddMemberResponse
                        {
                            Message = serviceResponse.Status
                        });

            return Ok(new AddMemberResponse
            {
                Message = serviceResponse.Status,
                Member = new()
                {
                    Id = serviceResponse.Member.Id.ToString(),
                    Name = serviceResponse.Member.OrganizationEmployee.User.FullName,
                    Role = serviceResponse.Member.Role,
                }
            });
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Remove(RemoveProjectMemberRequest request)
        {
            var userId = HttpContext.User.Claims
               .FirstOrDefault(c => c.Type.Equals("Id")).Value;

            var serviceResponse = await _projectMemberService
                .RemoveMember(request, userId);

            if (serviceResponse.Status.Equals(RemoveProjectMemberServiceResponseStatus.ProjectNotExists) ||
               serviceResponse.Status.Equals(RemoveProjectMemberServiceResponseStatus.MemberNotExists) ||
               serviceResponse.Status.Equals(RemoveProjectMemberServiceResponseStatus.AccessDenied) ||
               serviceResponse.Status.Equals(RemoveProjectMemberServiceResponseStatus.LeaderCanNotRemoved))
                return StatusCode(StatusCodes.Status400BadRequest,
                        new RemoveProjectMemberResponse
                        {
                            Message = serviceResponse.Status
                        });

            if (serviceResponse.Status.Equals(AddMemberServiceResponseStatus.InternalError))
                return StatusCode(StatusCodes.Status500InternalServerError,
                        new RemoveProjectMemberResponse
                        {
                            Message = serviceResponse.Status
                        });

            return Ok(new RemoveProjectMemberResponse
            {
                Message = serviceResponse.Status
            });
        }

        [Authorize]
        [HttpPatch]
        public async Task<IActionResult> ChangeRole(ChangeProjectMemberRoleRequest request)
        {
            var userId = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type.Equals("Id")).Value;

            var serviceResponse = await _projectMemberService
                .ChangeMemberRole(request, userId);

            if (serviceResponse.Status.Equals(ChangeProjectMemberRoleServiceResponseStatus.ProjectNotExists) ||
                serviceResponse.Status.Equals(ChangeProjectMemberRoleServiceResponseStatus.MemberNotExists)||
                serviceResponse.Status.Equals(ChangeProjectMemberRoleServiceResponseStatus.AccessDenied)||
                serviceResponse.Status.Equals(ChangeProjectMemberRoleServiceResponseStatus.LeaderRoleCanNotChanged))
                return StatusCode(StatusCodes.Status400BadRequest,
                        new ChangeEmployeeRoleResponse
                        {
                            Message = serviceResponse.Status
                        });

            if (serviceResponse.Status.Equals(ChangeProjectMemberRoleServiceResponseStatus.InternalError))
                return StatusCode(StatusCodes.Status500InternalServerError,
                        new ChangeEmployeeRoleResponse
                        {
                            Message = serviceResponse.Status
                        });

            return Ok(new ChangeEmployeeRoleResponse
            {
                Message = serviceResponse.Status
            });
        }
    }
}
