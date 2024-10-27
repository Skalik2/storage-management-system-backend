using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace storage_management_system.Model.Entities
{
    [Table("User")]
    public class User
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Username { get; set; } = string.Empty;
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public bool Administrative { get; set; } = false;
        public bool Service { get; set; } = false;
        public string Email { get; set; } = string.Empty;
        public required string Password { get; set; }

        public required Company Company { get; set; }
        public required ICollection<Access> Accesses { get; set; }
        public required ICollection<UserAction> UserActions { get; set; } 
    }
}