using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace storage_management_system.Model.Entities
{
    [Table("Storage")]
    public class Storage
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        [StringLength(50)]
        public string Model { get; set; } = "Custom";
        public required Company Company { get; set; }
        public int? LocationId { get; set; }
        public Location? Location { get; set; }
        public ICollection<Row>? Rows { get; set; }
    }
}
