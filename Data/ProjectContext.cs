using Microsoft.EntityFrameworkCore;
#pragma warning disable

namespace ProjectManager.Data
{
    public class ProjectContext : DbContext
    {
        public ProjectContext(DbContextOptions<ProjectContext> options) : base(options)
        { }
        public DbSet<Account> Account { get; set; }
        public DbSet<Profile> Profile { get; set; }
        public DbSet<Meeting> Meeting { get; set; }
        public DbSet<Invitation> Invitation { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<Phase> Phase { get; set; }
        public DbSet<WorkItem> WorkItem { get; set; }
        public DbSet<Activity> Activity { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().ToTable("Account");
            modelBuilder.Entity<Profile>().ToTable("Profile");
            modelBuilder.Entity<Invitation>().ToTable("Invitation");
            modelBuilder.Entity<Meeting>().ToTable("Meeting");
            modelBuilder.Entity<Project>().ToTable("Project");
            modelBuilder.Entity<Phase>().ToTable("Phase");
            modelBuilder.Entity<WorkItem>().ToTable("WorkItem");
            modelBuilder.Entity<Activity>().ToTable("Activity");
        }
    }
}
