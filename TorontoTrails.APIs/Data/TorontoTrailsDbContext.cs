using Microsoft.EntityFrameworkCore;
using TorontoTrails.APIs.Models.Domain;

namespace TorontoTrails.APIs.Data
{
    public class TorontoTrailsDbContext : DbContext
    {
        public TorontoTrailsDbContext(DbContextOptions dbContextOptions): base(dbContextOptions)
        {
            
        }

        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Trails> Trails { get; set; }

    }
}
