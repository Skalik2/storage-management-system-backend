using System.ComponentModel.DataAnnotations.Schema;

namespace storage_management_system.Model.Entities
{
    [Table("Item")]
    public class Item
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string Description { get; set; } = "No description";

        public required ICollection<ItemInstance> ItemInstances { get; set; }    
    }
}
