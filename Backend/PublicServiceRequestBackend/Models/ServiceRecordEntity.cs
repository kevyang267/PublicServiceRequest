namespace PublicServiceRequestBackend.Models
{
    public class ServiceRecordEntity
    {
        public int Id { get; set; }
        public string RequesterName { get; set; } = string.Empty;
        public string RequestType { get; set; } = string.Empty; // "Permit", "Complaint", "Information", "Other"
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = "Open"; // "Open", "In Progress", "Closed"
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
    }
}