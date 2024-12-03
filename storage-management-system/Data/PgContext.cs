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

        public DbSet<Company> Companies { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Storage> Storages { get; set; }
        public DbSet<Row> Rows { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Box> Boxes { get; set; }
        public DbSet<Access> Accesses { get; set; }
        public DbSet<UserAction> UserActions { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemInstance> ItemInstances { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<ItemPicture> ItemPictures { get; set; }

    }
}
