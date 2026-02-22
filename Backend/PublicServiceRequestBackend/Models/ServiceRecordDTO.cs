using System.ComponentModel.DataAnnotations;

namespace PublicServiceRequestBackend.Models
{
    public class ServiceRecordDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Requester name must be between 1 and 100 characters.")]
        [Display(Name = "Requester Name")]
        public string RequesterName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Request Type")]
        public string RequestType { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Status { get; set; } = "Open";
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}