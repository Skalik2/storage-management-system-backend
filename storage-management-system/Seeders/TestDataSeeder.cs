﻿using storage_management_system.Data;
using storage_management_system.Model.Entities;

namespace storage_management_system.Seeders
{
    public class TestDataSeeder(PgContext context)
    {
        private readonly PgContext _context = context;

        public void Seed()
        {
            SeedCompanies();
            SeedUsers();
            SeedLocations();
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
                        LastName = "Nowak",
                        Password = "123456",
                        Email = "miroslawnowak@gmail.com"
                    },
                    new() {
                        Company = company,
                        FirstName = "Wiktor",
                        LastName = "Kowal",
                        Password = "654321",
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
                        Address = "Rzeszów, kwiatowa 5 35-121"
                    },
                    new() {
                        Address = "Kraków, przemysłowa 10 21-212"
                    }
                ];
                _context.Locations.AddRange(locations);
                _context.SaveChanges();
            }
        }
    }
}
