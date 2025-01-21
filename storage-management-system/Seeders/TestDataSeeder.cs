using storage_management_system.Data;
using storage_management_system.Model.Entities;
using storage_management_system.Services;

namespace storage_management_system.Seeders
{
    public class TestDataSeeder(PgContext context, IPasswordHasher passwordHasher)
    {
        private readonly PgContext _context = context;
        private IPasswordHasher _passwordHasher = passwordHasher;

        public void Seed()
        {
            SeedCompanies();
            SeedUsers();
            SeedLocations();
            SeeedUserRoles();
        }

        private void SeeedUserRoles()
        {
            if (!_context.UserRole.Any())
            {
                IEnumerable<UserRole> userRoles =
                [
                    new UserRole {
                        UserId = 1,
                        RoleId = 1
                    }
                ];

                _context.UserRole.AddRange(userRoles);
                _context.SaveChanges();
            }
        }

        private void SeedCompanies()
        {
            if (!_context.Companies.Any())
            {
                IEnumerable<Company> companies =
                [
                    new Company { Name = "SuperWiertarki sp. Z.O.O" },
                    new Company { Name = "Maszinery" },
                ];

                _context.Companies.AddRange(companies);
                _context.SaveChanges();
            }
        }

        private void SeedUsers()
        {
            if (!_context.Users.Any())
            {
                var company = _context.Companies.First();
                IEnumerable<User> users =
                [
                    new() {
                        Company = company,
                        FirstName = "Mirosław",
                        Username = "admin",
                        LastName = "Nowak",
                        Password = _passwordHasher.Hash("admin"),
                        Email = "test@test.com"
                    },
                    new() {
                        Company = company,
                        FirstName = "Wiktor",
                        LastName = "Kowal",
                        Password = _passwordHasher.Hash("admin"),
                        Email = "wiktorkowal@gmail.com"
                    }
                ];
                _context.Users.AddRange(users);
                _context.SaveChanges();
            }
        }

        private void SeedLocations()
        {
            if (!_context.Locations.Any())
            {
                IEnumerable<Location> locations =
                [
                    new() {
                        Address = "Rzeszów, Kwiatowa 5, 35-121"
                    },
                    new() {
                        Address = "Kraków, Przemysłowa 10, 21-212"
                    }
                ];
                _context.Locations.AddRange(locations);
                _context.SaveChanges();
            }
        }
    }
}
