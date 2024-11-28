using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace storage_management_system.Model.Entities
{
    [Table("Company")]
    public class Company
    {
        public int Id { get; set; }
        [StringLength(50)]
        public required string Name { get; set; }

        public ICollection<User>? Users { get; set; }
        public ICollection<Storage>? Storages { get; set; }
    }
}
