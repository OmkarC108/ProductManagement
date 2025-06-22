using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Models
{
    public class Category
    {
        public int CategoryID { get; set; }

        [Required (ErrorMessage = "Category name is required")]
        public string CategoryName { get; set; }

        public virtual ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
    }
}
