using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Domain.Entities.Common;
using Domain.Entities.HumanResource;
using Domain.Entities.Project;
using Domain.Entities.Project.ProjectTask;
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
       
        //Common
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<InvitationPending> InvitationPendings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {           
            modelBuilder.Entity<Project>()
                .HasMany(o => o.ProjectMembers)             
                  .WithOne(pm => pm.Project)                 
                    .HasForeignKey(pm => pm.ProjectId)
                       .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
