namespace SquaresAPI.Models
{
    /// <summary>
    /// 2D Plane that hold points
    /// </summary>
    public class Plane
    {
        public int Id { get; set; }
        public List<Point>? Points { get; set; }
    }
}
