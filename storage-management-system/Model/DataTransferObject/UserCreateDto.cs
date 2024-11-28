namespace storage_management_system.Model.DataTransferObject
{
    public class UserCreateDto
    {
        public string Username { get; set; } = string.Empty;
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string Email { get; set; } = string.Empty;
        public required string Password { get; set; }
        public int CompanyId { get; set; }
    }
}
