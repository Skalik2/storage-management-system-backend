using System.ComponentModel.DataAnnotations.Schema;

namespace storage_management_system.Model.Entities
{
    [Table("Row")]
    public class Row
    {
        public int Id { get; set; }
        public required int StorageId { get; set; }
        public required Storage Storage { get; set; }
        public ICollection<Section>? Sections { get; set; }
    }
}
