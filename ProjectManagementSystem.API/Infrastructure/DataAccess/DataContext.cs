using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.HumanResource;
using Domain.Entities.HumanResource.Team;
using Domain.Entities.Project;
namespace Infrastructure.DataAccess
{
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        //Project entities
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectManagement> ProjectManagements { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }
        public DbSet<ProjectTaskExecutiveAgent> ProjectTaskExecutiveAgents { get; set; }

        //HumanResources entities
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }

    }
}
