using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace storage_management_system.Model.Entities
{
    [Table("Location")]
    public class Location
    {
        public int Id { get; set; }
        [StringLength(100)]
        public required string Address { get; set; }
        public ICollection<Storage>? Storages { get; set; }
    }
}
