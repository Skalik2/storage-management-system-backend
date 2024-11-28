using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace storage_management_system.Model.Entities
{
    [Table("Item")]
    public class Item
    {
        public int Id { get; set; }
        [StringLength(50)]
        public required string Name { get; set; }
        [StringLength(256)]
        public string Description { get; set; } = "No description";

        public ICollection<ItemInstance>? ItemInstances { get; set; }    
    }
}
