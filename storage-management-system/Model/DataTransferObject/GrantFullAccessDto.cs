namespace storage_management_system.Model.DataTransferObject
{
    public class GrantFullAccessDto
    {
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public int StorageId { get; set; }
    }
}
