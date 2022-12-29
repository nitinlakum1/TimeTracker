using Microsoft.EntityFrameworkCore;
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
    }
}