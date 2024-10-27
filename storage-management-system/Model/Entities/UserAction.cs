using System.ComponentModel.DataAnnotations.Schema;

namespace storage_management_system.Model.Entities
{
    [Table("UserAction")]
    public class UserAction
    {
        public int Id { get; set; }
        public int BoxId { get; set; }
        public int UserId { get; set; }
        public int OperationId { get; set; }
        public int Quantity { get; set; } = 0;
        public TimeSpan Time {  get; set; }
        public required User User { get; set; }
        public required Box Box { get; set; }
        public required Operation Operation { get; set; }
    }
}
