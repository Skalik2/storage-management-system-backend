namespace storage_management_system.Model.DataTransferObject
{
    public class AssignAccessDto
    {
        public int UserId { get; set; }
        public required List<int> BoxIds { get; set; } 
    }
}
