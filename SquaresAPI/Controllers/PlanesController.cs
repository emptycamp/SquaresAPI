using Microsoft.AspNetCore.Mvc;
using SquaresAPI.Models;
using SquaresAPI.Repositories;
using SquaresAPI.Services;

namespace SquaresAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class PlanesController : ControllerBase
    {
        private readonly IPlaneRepository _planeRepo;

        public PlanesController(IPlaneRepository planeRepo)
        {
            _planeRepo = planeRepo;
        }

        /// <summary>
        /// Gets all 2D Planes from database without including points
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<Plane>>> GetPlanes()
        {
            return await _planeRepo.GetPlanes();
        }


        /// <summary>
        /// Gets plane by id including all of its points
        /// </summary>
        /// <param name="id">Id of the 2D plane</param>
        /// <response code="404">If 2D plane doesn't exist</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Plane>> GetPlane(int id)
        {
            var plane = await _planeRepo.GetPlane(id);

            if (plane == null)
            {
                return NotFound();
            }

            return plane;
        }

        /// <summary>
        /// Finds all squares in specified 2D plane
        /// </summary>
        /// <param name="id">Id of the 2D plane</param>
        /// <response code="404">If 2D plane doesn't exist</response>
        [HttpGet("solve/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FoundSquaresDTO>> GetSquares(int id)
        {
            var plane = await _planeRepo.GetPlane(id);

            if (plane == null)
            {
                return NotFound();
            }

            var points = plane.Points?.ToArray() ?? Array.Empty<Point>();
            var squares = PlaneSolver.FindSquares(points);

            return new FoundSquaresDTO { Count = squares.Length, Squares = squares };
        }

        /// <summary>
        /// Adds points to the given 2D plane
        /// </summary>
        /// <param name="id">Id of the 2D canvas</param>
        /// <param name="points">Points to be added</param>
        /// <remarks>
        /// Add points:
        ///
        ///     POST /api/Planes/{id}
        ///     [
        ///       {
        ///         "x": -1,
        ///         "y": 0
        ///       },
        ///       {
        ///         "x": 1,
        ///         "y": 0
        ///       }
        ///     ]
        /// </remarks>
        /// <response code="404">If 2D plane doesn't exist</response>
        [HttpPost("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Plane>> AddPoints(int id, List<Point> points)
        {
            var plane = await _planeRepo.AddPoints(id, points);

            if (plane == null)
            {
                return NotFound();
            }

            return plane;
        }

        /// <summary>
        /// Creates 2D plane with specified points
        /// </summary>
        /// <param name="plane"></param>
        /// <remarks>
        /// Import multiple points:
        ///
        ///     POST /api/Planes
        ///     {
        ///       "points":
        ///         [
        ///           {
        ///             "x": -1,
        ///             "y": 0
        ///           },
        ///           {
        ///             "x": 1,
        ///             "y": 0
        ///           }
        ///         ]
        ///     }
        ///
        /// Create empty plane:
        /// 
        ///     POST /api/Planes
        ///     {
        ///       "points": []
        ///     }
        /// </remarks>
        [HttpPost]
        public async Task<ActionResult<Plane>> AddPlane(Plane plane)
        {
            await _planeRepo.CreatePlane(plane);

            return CreatedAtAction("GetPlane", new { id = plane.Id }, plane);
        }

        /// <summary>
        /// Deletes point from 2D plane
        /// </summary>
        /// <param name="id">Id of the point</param>
        [HttpDelete("point/{id}")]
        public async Task<IActionResult> DeletePoint(int id)
        {
            await _planeRepo.DeletePoint(id);
            return NoContent();
        }

        /// <summary>
        /// Deletes 2D plane with all of the points inside of it
        /// </summary>
        /// <param name="id">Id of the 2D plane</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlane(int id)
        {
            await _planeRepo.DeletePlane(id);
            return NoContent();
        }
    }
}
