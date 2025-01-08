namespace storage_management_system.Services
{
    public interface IPasswordHasher
    {
        string Hash(string password);
        bool Verification(string password, string passwordHash);
    }
}
