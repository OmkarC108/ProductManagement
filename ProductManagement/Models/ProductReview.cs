using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Models
{
    public class ProductReview
    {
        [Key]
        public int ReviewID { get; set; }

        [Required]
        public int ProductID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }

        public string? Comment { get; set; }
        public string? Sentiment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

}
