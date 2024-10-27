using System.ComponentModel.DataAnnotations.Schema;

namespace storage_management_system.Model.Entities
{
    [Table("Location")]
    public class Location
    {
        public int Id { get; set; }
        public required string Address { get; set; }
        public required ICollection<Storage> Storages { get; set; }
    }
}
