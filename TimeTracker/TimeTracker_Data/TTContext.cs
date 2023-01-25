using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using TimeTracker_Data.Model;

namespace TimeTracker_Data
{
    public class TTContext : DbContext
    {
        public TTContext(DbContextOptions<TTContext> options)
            : base(options)
        { }

        public DbSet<Users> Users { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<SystemLogs> SystemLogs { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Holidays> Holidays { get; set; }
        public DbSet<Salarys> Salarys { get; set; }
        public DbSet<Resources> Resources { get; set; }
        public DbSet<ResourcesFollowup> ResourcesFollowup { get; set; }
        public DbSet<ErrorLogs> ErrorLogs { get; set; }
        public DbSet<SalaryReports> SalaryReports { get; set; }
        public DbSet<Leaves> Leaves { get; set; }
        public DbSet<UpdateServices> UpdateServices { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>()
               .HasOne(g => g.Roles)
               .WithOne()
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SystemLogs>()
               .HasOne(g => g.Users)
               .WithOne()
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Salarys>()
               .HasOne(g => g.Users)
               .WithOne()
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SalaryReports>()
               .HasOne(g => g.Users)
               .WithOne()
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Leaves>()
               .HasOne(g => g.Users)
               .WithOne()
               .OnDelete(DeleteBehavior.NoAction);

        }
    }
}