using System.ComponentModel.DataAnnotations.Schema;

namespace storage_management_system.Model.Entities
{
    [Table("Box")]
    public class Box
    {
        public int Id { get; set; }
        public int SectionId {  get; set; }
        public required Section Section { get; set; }
        public required ICollection<Access> Accesses { get; set; }
        public required ICollection<UserAction> UserActions { get; set; }
    }
}
