using storage_management_system.Data;
using storage_management_system.Model.Entities;

namespace storage_management_system.Seeders
{
    public class CoreDataSeeder
    {
        private readonly PgContext _context;

        public CoreDataSeeder(PgContext context)
        {
            _context = context;
        }

        public void SeedCoreData()
        {
            SeedOperations();
            SeedRoles();
        }

        private void SeedRoles()
        {
            if (!_context.Role.Any())
            {
                IEnumerable<Role> roles = new List<Role>
                {
                    new Role { Name = "RootAdmin"},
                    new Role { Name = "HeadAdmin"},
                    new Role { Name = "Admin"},
                    new Role { Name = "Service"},
                    new Role { Name = "User"},
                };

                _context.Role.AddRange(roles);
                _context.SaveChanges();
            }
        }

        private void SeedOperations()
        {
            if (!_context.Operations.Any())
            {
                IEnumerable<Operation> operations = new List<Operation>
                {
                    new Operation { Name = "Login"},
                    new Operation { Name = "CreateUser"},
                    new Operation { Name = "CreateItem"},
                    new Operation { Name = "DeleteItem"},
                    new Operation { Name = "AssignItemToBox"},
                    new Operation { Name = "AssignRole"},
                    new Operation { Name = "RevokeRole"},
                    new Operation { Name = "AssignAccessToBox"},
                    new Operation { Name = "RevokeAccessToBox"},
                    new Operation { Name = "CreateStorage"},
                    new Operation { Name = "RemoveStorage"},
                    new Operation { Name = "AddLocation"},
                    new Operation { Name = "RemoveLocation"},
                };

                _context.Operations.AddRange(operations);
                _context.SaveChanges();
            }
        }
    }
}
