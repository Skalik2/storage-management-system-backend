using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace storage_management_system.Model.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public int Company_id { get; set; }
        public string Username { get; set; }
        [Required]
        public string First_name { get; set; }
        [Required]
        public string Last_name { get; set; }
        public bool Administrative { get; set; }
        public bool Service {  get; set; }
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}