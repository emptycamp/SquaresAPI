using Microsoft.EntityFrameworkCore;

namespace SquaresAPI.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Plane>().HasMany(p => p.Points).WithOne().OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<Plane> Plane => Set<Plane>();

        public DbSet<Point> Points => Set<Point>();
    }
}
