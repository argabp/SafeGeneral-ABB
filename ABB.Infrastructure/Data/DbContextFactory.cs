using ABB.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ABB.Infrastructure.Data
{
    public class DbContextFactory : IDbContextFactory
    {
        private readonly IConfiguration _configuration;

        public DbContextFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Implementation of the interface method
        public IDbContext CreateDbContext(string databaseName)
        {
            var connectionString = string.Format(_configuration.GetConnectionString("UserConnection"), databaseName);

            var options = new DbContextOptionsBuilder<ABBDbContext>()
                .UseSqlServer(connectionString) // You can replace with UseSqlite, UseNpgsql, etc.
                .Options;

            return new ABBDbContext(options);
        }
    }
}