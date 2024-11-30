namespace storage_management_system.Model.DataTransferObject
{
    public class PredefinedStorageDto
    {
        public int CompanyId { get; set; }
        public int LocationId { get; set; }
        public string Model { get; set; } = "Custom";
    }
}
