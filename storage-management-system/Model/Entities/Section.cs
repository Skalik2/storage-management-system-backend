using System.ComponentModel.DataAnnotations.Schema;

namespace storage_management_system.Model.Entities
{
    [Table("Section")]
    public class Section
    {
        public int Id { get; set; }

        public int RowId { get; set; }
        public required Row Row { get; set; }
        public ICollection<Box>? Boxes { get; set; }
    }
}
