using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SquaresAPI.Models;

namespace SquaresAPI.Test
{
    [TestClass]
    public class PlanesControllerTests : IntegrationTest
    {
        [TestMethod]
        public async Task GetPlanes_EmptyDb()
        {
            var response = await Client.GetAsync("api/Planes");
            var planes = await response.Content.ReadAsAsync<List<Plane>>();

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsTrue(planes.All(x => x.Points == null));
            Assert.IsFalse(planes.Any());
        }

        [TestMethod]
        public async Task GetPlane_GetsPlaneById()
        {
            var newPlane = await AddPlane();

            var response = await Client.GetAsync($"api/Planes/{newPlane.Id}");
            var plane = await response.Content.ReadAsAsync<Plane>();

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(newPlane.Id, plane.Id);
            Assert.IsFalse(plane.Points?.Any());
        }


        [TestMethod]
        public async Task AddPlane_ImportPoints()
        {

            var points = new List<Point>
            {
                new() { X = -1, Y = -1 },
                new() { X = -1, Y = 1 },
                new() { X = 1, Y = 1 },
                new() { X = 1, Y = -1 }
            };

            var planeModel = new Plane
            {
                Points = points
            };

            var plane = await AddPlane(planeModel);

            Assert.AreEqual(1, plane.Id);
            Assert.IsTrue(planeModel.Points.SequenceEqual(plane.Points));
        }

        [TestMethod]
        public async Task AddPoints_AddTwoPointsToPlane()
        {
            var emptyPlane = await AddPlane();

            var points = new List<Point>
            {
                new() { X = -1, Y = -1 },
                new() { X = -1, Y = 1 }
            };


            Assert.AreEqual(emptyPlane.Points, null);

            var response = await Client.PostAsJsonAsync($"api/Planes/{emptyPlane.Id}", points);
            var updatedPlane = await response.Content.ReadAsAsync<Plane>();

            Assert.IsTrue(updatedPlane.Points.SequenceEqual(points));
        }

        [TestMethod]
        public async Task GetSquares_FindTwoSquares()
        {
            var points = new List<Point>
            {
                new() { X = -1, Y = -1 },
                new() { X = -1, Y = 1 },
                new() { X = 1, Y = 1 },
                new() { X = 1, Y = -1 },
                new() { X = 3, Y = -1 },
                new() { X = 3, Y = 1 }
            };

            var plane = await AddPlane(new Plane{Points = points});

            var response = await Client.GetAsync($"api/Planes/solve/{plane.Id}");
            var foundSquares = await response.Content.ReadAsAsync<FoundSquaresDTO>();

            Assert.AreEqual(2, foundSquares.Count);
        }

        private async Task<Plane> AddPlane(Plane? planeModel = null)
        {
            planeModel ??= new Plane();

            var response = await Client.PostAsJsonAsync("api/Planes", planeModel);
            var plane = await response.Content.ReadAsAsync<Plane>();

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            return plane;
        }
    }
}
