using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Domain.Entities.Common;
using Domain.Entities.HumanResource;
using Domain.Entities.Project;
using Domain.Entities.Project.ProjectTask;
namespace Infrastructure.DataAccess
{
    /// <summary>
    /// This class (that drives EFCore's Dbcontext class) is base of all communications with database in this project
    /// NOTE : In this project "IdentityDbContext" is used instead Dbcontext for adding some auth and security options
    /// </summary>
    /// <param name="options"></param>
    public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        /// <summary>
        /// Entities configuration (This entities also exists as a table in database)
        /// NOTE:All of these entities are defined in Entities folder in Domain layer and all relationships are defined in entities themselves
        /// </summary>

        //HumanResource
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<OrganizationEmployee> OrganizationEmployees { get; set; }
        public DbSet<ProjectMember> ProjectMembers { get; set; }

        //Project
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTaskList> ProjectTaskLists { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }
        public DbSet<TaskAssignment> TaskAssignments { get; set; }
        public DbSet<TaskComment> TaskComments { get; set; }
        public DbSet<TaskLable> TaskLables { get; set; }

        //Common
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<InvitationPending> InvitationPendings { get; set; }
        public DbSet<StorageItemUrl> StorageItemsUrls { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>()
                .HasMany(o => o.ProjectMembers)
                  .WithOne(pm => pm.Project)
                    .HasForeignKey(pm => pm.ProjectId)
                       .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProjectMember>()
                .HasMany(m => m.TaskAssignments)
                    .WithOne(a => a.Member)
                        .HasForeignKey(a => a.ProjectMemberId)
                            .OnDelete(DeleteBehavior.Restrict);


            base.OnModelCreating(modelBuilder);
        }
    }
}
