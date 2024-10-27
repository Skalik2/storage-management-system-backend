using System.ComponentModel.DataAnnotations.Schema;

namespace storage_management_system.Model.Entities
{
    [Table("Storage")]
    public class Storage
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Model { get; set; } = "Custom";
        public required Company Company { get; set; }
        public required Location Location { get; set; }
        public required ICollection<Row> Rows { get; set; }
    }
}
