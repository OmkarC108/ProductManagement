using Microsoft.AspNetCore.Mvc;
using ProductManagement.Models;

namespace ProductManagement.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context) 
        {
            _context = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PlaceOrder(int productId, int quantity)
        {
            var product = _context.Products.Find(productId);

            if (product == null)
            {
                TempData["Message"] = "Product not found.";
            }
            else if (quantity <= 0)
            {
                TempData["Message"] = "Invalid quantity.";
            }
            else if (product.StockQuantity >= quantity)
            {
                product.StockQuantity -= quantity;
                _context.SaveChanges();
                TempData["Message"] = "Order placed successfully!";
            }
            else
            {
                TempData["Message"] = "Insufficient stock.";
            }

            return RedirectToAction("Details", "Product", new { id = productId });
        }
    }
}
