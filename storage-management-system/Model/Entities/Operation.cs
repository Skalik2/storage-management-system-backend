using System.ComponentModel.DataAnnotations.Schema;

namespace storage_management_system.Model.Entities
{
    [Table("Operation")]
    public class Operation
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string Description { get; set; } = "No description";

        public required ICollection<UserAction> UserActions { get; set; }
    }
}
