using SquaresAPI.Models;

namespace SquaresAPI.Services
{
    public class PlaneSolver
    {
        // 
        /// <summary>
        /// Finds all squares in 2D plane given n points.
        /// Inspired by https://stackoverflow.com/questions/3831144/finding-the-squares-in-a-plane-given-n-points.
        /// </summary>
        /// <param name="points">Point that includes x and y coordinates</param>
        /// <returns>List of four corner points that identifies the square</returns>
        public static Point[][] FindSquares(Point[] points)
        {
            var pointSet = points.ToHashSet();
            var squarePoints = new Dictionary<int, Point[]>();

            for (var i = 0; i < pointSet.Count; i++)
            {
                for (var j = 0; j < pointSet.Count; j++)
                {
                    if (i == j)
                        continue;

                    // Checks if diagonal vertex of two selected points exists in our set.
                    var diagVertex = GetRestPints(points[i], points[j]);

                    if (pointSet.Contains(diagVertex.b) && pointSet.Contains(diagVertex.d))
                    {
                        var square = new[] { points[i], points[j], diagVertex.b, diagVertex.d };

                        // Calculates hash of coordinates, since it can find same square again.
                        var hash = square.Select(pt => pt.GetHashCode()).Aggregate((a, b) => a ^ b);
                        squarePoints[hash] = square;
                    }
                }
            }

            return squarePoints.Values.ToArray();
        }

        /// <summary>
        /// Calculates remaining two points from diagonal vertex.
        /// 
        /// Example:
        ///     a---b
        ///     |   |   Given diagonal vertex of a and c, returns b and d
        ///     d---c
        /// 
        /// </summary>
        /// <param name="a">Point a</param>
        /// <param name="c">Point c that forms diagonal with point a</param>
        /// <returns>Coordinates of remaining points b and d</returns>
        private static (Point b, Point d) GetRestPints(Point a, Point c)
        {
            var midX = (a.X + c.X) / 2;
            var midY = (a.Y + c.Y) / 2;

            var Ax = a.X - midX;
            var Ay = a.Y - midY;
            var bX = midX - Ay;
            var bY = midY + Ax;
            var b = new Point { X = bX, Y = bY };

            var cX = (c.X - midX);
            var cY = (c.Y - midY);
            var dX = midX - cY;
            var dY = midY + cX;
            var d = new Point { X = dX, Y = dY };

            return (b, d);
        }
    }
}
