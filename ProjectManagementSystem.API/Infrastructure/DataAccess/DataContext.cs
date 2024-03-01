using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.HumanResource;
using Domain.Entities.Project.ProjectTask;
using Domain.Entities.Project.ProjectMemberTask;
using Domain.Entities.Project;
using Domain.Entities.Common;
namespace Infrastructure.DataAccess
{
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        //HumanResource
        public DbSet<User> Users { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<OrganizationEmployee> organizationEmployees { get; set; }
        public DbSet<ProjectMember> MyProperty { get; set; }

        //Project
        public DbSet<Project> projects { get; set; }
        public DbSet<ProjectTaskList> projectTaskLists { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }
        public DbSet<ProjectMemberTaskList> ProjectMemberTaskLists { get; set; }
        public DbSet<ProjectMemberTask> MyProperty1 { get; set; }

        //Common
        public DbSet<Notification> Notifications{ get; set; }



    }
}
