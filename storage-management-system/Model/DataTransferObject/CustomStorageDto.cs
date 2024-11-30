namespace storage_management_system.Model.DataTransferObject
{
    public class CustomStorageDto
    {
        public int CompanyId { get; set; }
        public int LocationId { get; set; }

        public required int RowCount { get; set; }
        public required int SectionCount { get; set; }
        public required int BoxCount { get; set; }

    }
}
