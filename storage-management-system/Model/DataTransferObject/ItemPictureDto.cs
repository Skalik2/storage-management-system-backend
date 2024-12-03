namespace storage_management_system.Model.DataTransferObject
{
    public class ItemPictureDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<IFormFile>? Picture { get; set; }
    }
}
