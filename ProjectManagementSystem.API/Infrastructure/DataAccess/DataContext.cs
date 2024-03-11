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
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace Infrastructure.DataAccess
{
    public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        //HumanResource
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<OrganizationEmployee> OrganizationEmployees { get; set; }
        public DbSet<ProjectMember> ProjectMembers { get; set; }

        //Project
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTaskList> ProjectTaskLists { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }
        public DbSet<ProjectMemberTaskList> ProjectMemberTaskLists { get; set; }
        public DbSet<ProjectMemberTask> ProjectMemberTasks { get; set; }

        //Common
        public DbSet<Notification> Notifications{ get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            //{
            //    relationship.DeleteBehavior = DeleteBehavior.Restrict;
            //}
            base.OnModelCreating(modelBuilder);
        }

    }
}
