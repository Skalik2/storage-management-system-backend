namespace storage_management_system.Model.DataTransferObject
{
    public class UserCreateBasicDto
    {
        public string? Username { get; set; } = string.Empty;
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public int CompanyId { get; set; }
    }
}
