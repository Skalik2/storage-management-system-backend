using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace storage_management_system.Model.Entities
{
    [Table("Role")]
    public class Role
    {
        public int Id { get; set; }
        [StringLength(20)]
        public required string Name { get; set; }

        public ICollection<UserRole>? UserRoles { get; set; }
    }
}
