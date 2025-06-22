using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Models;

public class ProductController : Controller
{
    private readonly AppDbContext _context;

    public ProductController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index(string name, int? categoryId, decimal? minPrice, decimal? maxPrice)
    {
        var products = _context.Products
            .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
            .AsQueryable();

        if (!string.IsNullOrEmpty(name))
            products = products.Where(p => p.Name.Contains(name) || p.Description.Contains(name));

        if (categoryId.HasValue)
            products = products.Where(p => p.ProductCategories.Any(pc => pc.CategoryID == categoryId));

        if (minPrice.HasValue)
            products = products.Where(p => p.Price >= minPrice);

        if (maxPrice.HasValue)
            products = products.Where(p => p.Price <= maxPrice);

        ViewBag.Categories = _context.Categories.ToList();
        return View(products.ToList());
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Categories = _context.Categories.ToList();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Product product, int CategoryID)
    {
        if (ModelState.IsValid)
        {
            product.CreatedAt = DateTime.Now;
            product.UpdatedAt = DateTime.Now;

            _context.Products.Add(product);
            _context.SaveChanges();

            var productCategory = new ProductCategory
            {
                ProductID = product.ProductID,
                CategoryID = CategoryID
            };
            _context.ProductCategories.Add(productCategory);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        ViewBag.Categories = _context.Categories.ToList();
        return View(product);
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var product = _context.Products.Find(id);
        if (product == null) return NotFound();

        ViewBag.Categories = _context.Categories.ToList();
        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Product product)
    {
        if (ModelState.IsValid)
        {
            product.UpdatedAt = DateTime.Now;
            _context.Entry(product).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Categories = _context.Categories.ToList();
        return View(product);
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var product = _context.Products.Find(id);
        if (product == null) return NotFound();

        _context.Products.Remove(product);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        var product = _context.Products
            .Include(p => p.ProductReviews)
            .FirstOrDefault(p => p.ProductID == id);

        if (product == null) return NotFound();

        return View(product);
    }
}
