using Domain.Constants.Roles.OrganiationEmployees;
using Domain.Entities.HumanResource;
using Domain.Models.Dtos.ProjectTaskList.Request;
using Domain.Models.ServiceResponses.ProjectMember;
using Domain.Models.ServiceResponses.ProjectTaskList;
using Domain.Services.ApiServices;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.ApiServices
{
    public class ProjectTaskListService(DataContext context,
        ILogger<AuthenticationService> logger) : IProjectTaskListService
    {
        private readonly DataContext _context = context;
        private readonly ILogger<AuthenticationService> _logger = logger;

        public async Task<ProjectTaskListServiceResponse> CreateTaskList(CreateTaskListRequest request)
        {
            try
            {
                var project = await _context.Projects
                               .Include(p => p.ProjectMembers)
                                    .Include(p => p.ProjectTaskLists)
                                        .FirstOrDefaultAsync(p =>
                                             p.Id.ToString().Equals(request.ProjectId));

                if (project == null)
                    return new ProjectTaskListServiceResponse(
                         ProjectTaskListServiceResponseStatus.ProjectNotExists);

                var lastPriority = project.ProjectTaskLists
                    .Select(t => int.Parse(t.Priority))
                        .DefaultIfEmpty(0).Max();

                project.ProjectTaskLists.Add(new()
                {
                    Name = request.Name,
                    Priority = (++lastPriority).ToString()
                });

                await _context.SaveChangesAsync();

                return new ProjectTaskListServiceResponse(
                     ProjectTaskListServiceResponseStatus.Success);
            }
            catch (Exception ex)
            {
                _logger.LogError("CreateTaskListService : {Message}", ex.Message);

                return new ProjectTaskListServiceResponse(
                     ProjectTaskListServiceResponseStatus.InternalError);
            }
            
        }
    }
}
