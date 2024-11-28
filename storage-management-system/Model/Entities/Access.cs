using System.ComponentModel.DataAnnotations.Schema;

namespace storage_management_system.Model.Entities
{
    [Table("Access")]
    public class Access
    {
        public int Id { get; set; }

        public int BoxId { get; set; }
        public required Box Box { get; set; }
        public int UserId { get; set; }
        public required User User { get; set; }
    }
}
