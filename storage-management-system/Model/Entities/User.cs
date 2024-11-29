using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace storage_management_system.Model.Entities
{
    [Table("User")]
    public class User
    {
        public int Id { get; set; }
        [StringLength(20)]
        public string Username { get; set; } = string.Empty;
        [StringLength(20)]
        public required string FirstName { get; set; }
        [StringLength(20)]
        public required string LastName { get; set; }
        public bool Administrative { get; set; } = false;
        public bool Service { get; set; } = false;
        [StringLength(50)]
        public string Email { get; set; } = string.Empty;
        public required string Password { get; set; }


        public int CompanyId { get; set; }
        public Company? Company { get; set; }
        public ICollection<Access>? Accesses { get; set; }
        public ICollection<UserAction>? UserActions { get; set; } 
    }
}