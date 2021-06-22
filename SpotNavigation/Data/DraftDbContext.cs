using Microsoft.EntityFrameworkCore;
using SpotNavigation.Models;

namespace SpotNavigation.Data
{
    public class DraftDbContext:DbContext
    {
        // This constructor must exist so you can register it as a service (next slide)
        public DraftDbContext(DbContextOptions<DraftDbContext> options) : base(options)
        {
                
        }

        // Each DB set maps to a table in the database
        public DbSet<Draft> Drafts { get; set; }
        public DbSet<Solution> Paths { get; set; }
        
    }
}