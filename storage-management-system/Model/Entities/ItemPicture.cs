using System.ComponentModel.DataAnnotations.Schema;

namespace storage_management_system.Model.Entities
{
    [Table("ItemPicture")]
    public class ItemPicture
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string? ImageName { get; set; }
        public required string ImagePath { get; set; }
    }
}
