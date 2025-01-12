using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace storage_management_system.Model.Entities
{
    [Table("User")]
    [Index(nameof(Username), IsUnique = true)]
    public class User
    {
        public int Id { get; set; }
        [StringLength(20)]
        public string? Username { get; set; } = string.Empty;
        [StringLength(20)]
        public required string FirstName { get; set; }
        [StringLength(20)]
        public required string LastName { get; set; }
        [StringLength(50)]
        public required string Email { get; set; }
        public required string Password { get; set; }


        public int CompanyId { get; set; }
        public Company? Company { get; set; }
        public ICollection<UserRole>? UserRoles { get; set; }
        public ICollection<Access>? Accesses { get; set; }
        public ICollection<UserAction>? UserActions { get; set; } 
    }
}