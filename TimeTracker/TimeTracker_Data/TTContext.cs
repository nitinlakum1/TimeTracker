using Microsoft.EntityFrameworkCore;
using TimeTracker_Data.Model;

namespace TimeTracker_Data
{
    public class TTContext : DbContext
    {
        public TTContext(DbContextOptions<TTContext> options)
            : base(options)
        { }

        public TTContext() { }

        public DbSet<User> Users { get; set; }
    }
}