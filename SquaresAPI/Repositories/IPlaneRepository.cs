using SquaresAPI.Models;

namespace SquaresAPI.Repositories
{
    public interface IPlaneRepository
    {
        Task<List<Plane>> GetPlanes();
        Task<Plane?> GetPlane(int id);
        Task<Plane> CreatePlane(Plane plane);
        Task DeletePlane(int id);

        Task<Plane?> AddPoints(int id, IEnumerable<Point> points);
        Task DeletePoint(int id);
    }
}
