using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace storage_management_system.Model.Entities
{
    [Table("UserAction")]
    public class UserAction
    {
        public int Id { get; set; }
        public int? BoxId { get; set; }
        public int? UserId { get; set; }
        public int OperationId { get; set; }
        public int Quantity { get; set; } = 0;
        public DateTime Time {  get; set; }
        public string? Description { get; set; }
        public User? User { get; set; }
        public Box? Box { get; set; }
        public Operation? Operation { get; set; }
    }
}
