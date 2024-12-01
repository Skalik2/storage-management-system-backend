using System.ComponentModel.DataAnnotations;

namespace storage_management_system.Model.DataTransferObject
{
    public class LoginRequestDto
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }
}
