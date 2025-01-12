using System.ComponentModel.DataAnnotations.Schema;

namespace storage_management_system.Model.Entities
{
    [Table("UserRole")]
    public class UserRole
    {
        public int Id { get; set; }

        public int RoleId { get; set; }
        public Role? Role { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
