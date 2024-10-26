using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using storage_management_system.Model.Entities;

namespace storage_management_system.Data
{
    public class PgContext : DbContext
    {
        protected readonly IConfiguration _configuration;

        public PgContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DbConnection"));
        }

        public DbSet<User> Users { get; set; }
    }
}
