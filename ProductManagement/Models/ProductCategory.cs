using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Models
{
    public class ProductCategory
    {
        [Required]
        public int ProductID { get; set; }
        public Product? Product { get; set; }

        [Required(ErrorMessage = "Category ID is required")]
        public int CategoryID { get; set; }
        public Category? Category { get; set; }
    }
}
