using Microsoft.EntityFrameworkCore;
using SquaresAPI.Models;

namespace SquaresAPI.Repositories
{
    public class PlaneRepository : IPlaneRepository
    {
        private readonly DataContext _context;
        private readonly DbSet<Plane> _planes;
        private readonly DbSet<Point> _points;

        public PlaneRepository(DataContext context)
        {
            _context = context;
            _planes = context.Plane;
            _points = context.Points;
        }

        /// <summary>
        /// Gets all 2D planes
        /// </summary>
        public async Task<List<Plane>> GetPlanes()
        {
            return await _planes.ToListAsync();
        }

        /// <summary>
        /// Gets 2D plane by id
        /// </summary>
        /// <param name="id">Plane id</param>
        public async Task<Plane?> GetPlane(int id)
        {
            return await _planes.Include(x => x.Points).FirstOrDefaultAsync(p => p.Id == id);
        }

        /// <summary>
        /// Creates 2D plane
        /// </summary>
        public async Task<Plane> CreatePlane(Plane plane)
        {
            _planes.Add(plane);
            await _context.SaveChangesAsync();

            return plane;
        }

        /// <summary>
        /// Deletes 2D plane with all of the points in it
        /// </summary>
        /// <param name="id">Plane id</param>
        public async Task DeletePlane(int id)
        {
            var plane = await _planes.FindAsync(id);

            if (plane != null)
            {
                _planes.Remove(plane);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Adds points to the existing 2D plane
        /// </summary>
        /// <param name="id">Plane id</param>
        /// <param name="points">List of points</param>
        public async Task<Plane?> AddPoints(int id, IEnumerable<Point> points)
        {
            var plane = await GetPlane(id);

            if (plane != null)
            {
                plane.Points ??= new List<Point>();
                plane.Points.AddRange(points);
                await _context.SaveChangesAsync();
            }

            return plane;
        }

        /// <summary>
        /// Deletes point
        /// </summary>
        /// <param name="id">Point id</param>
        public async Task DeletePoint(int id)
        {
            var point = await _points.FindAsync(id);

            if (point != null)
            {
                _points.Remove(point);
                await _context.SaveChangesAsync();
            }
        }
    }
}
