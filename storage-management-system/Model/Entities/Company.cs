using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace storage_management_system.Model.Entities
{
    [Table("Company")]
    public class Company
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public required ICollection<User> Users { get; set; }
        public required ICollection<Storage> Storages { get; set; }
    }
}
