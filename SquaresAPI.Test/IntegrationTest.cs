using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SquaresAPI.Models;

namespace SquaresAPI.Test
{
    public abstract class IntegrationTest
    {
        protected HttpClient Client { get; }
        protected DataContext? Context { get; }

        private readonly WebApplicationFactory<Program> _factory;
        private Guid _dbGuid = Guid.NewGuid();

        protected IntegrationTest()
        {
            // Uses in-memory db for testing, each method has fresh in-memory db.
            _factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll(typeof(DbContextOptions<DataContext>));
                        services.AddDbContext<DataContext>(options =>
                        {
                            options.UseInMemoryDatabase(_dbGuid.ToString());
                        });
                    });
                });

            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory?.CreateScope();
            Context = scope?.ServiceProvider.GetService<DataContext>();

            Client = _factory.CreateClient();
        }

        [TestInitialize()]
        public void Startup()
        {
            // Resets database for every method
            _dbGuid = Guid.NewGuid();
        }
    }
}