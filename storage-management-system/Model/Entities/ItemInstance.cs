using System.ComponentModel.DataAnnotations.Schema;

namespace storage_management_system.Model.Entities
{
    [Table("ItemInstance")]
    public class ItemInstance
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int BoxId { get; set; }
        public required int Quantity { get; set; }
        
        public required Item Item { get; set; }
        public required Box Box { get; set; }
    }
}
