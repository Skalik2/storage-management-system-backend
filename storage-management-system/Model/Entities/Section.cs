using System.ComponentModel.DataAnnotations.Schema;

namespace storage_management_system.Model.Entities
{
    [Table("Section")]
    public class Section
    {
        public int Id { get; set; }
        public int SectionId { get; set; }
        public required Row Row { get; set; }

        public required ICollection<Box> Boxes { get; set; }
    }
}
