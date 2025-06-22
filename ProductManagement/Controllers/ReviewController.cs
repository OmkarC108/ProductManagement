using Microsoft.AspNetCore.Mvc;
using ProductManagement.Models;
using System;
using System.Linq;

namespace ProductManagement.Controllers
{
    public class ReviewController : Controller
    {
        private readonly AppDbContext _context;

        public ReviewController(AppDbContext context)
        {
            _context = context;
        }

        //private string GetSentiment(string comment)
        //{
        //    var text = comment.ToLower();

        //    if (text.Contains("bad") || text.Contains("poor") || text.Contains("worst"))
        //        return "Negative";

        //    if (text.Contains("good") || text.Contains("excellent") || text.Contains("great"))
        //        return "Positive";

        //    return "Neutral";
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddReview(ProductReview review)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                TempData["ReviewMessage"] = "Invalid review submitted: " + string.Join("; ", errors);
                return RedirectToAction("Details", "Product", new { id = review.ProductID });
            }

            review.CreatedAt = DateTime.Now;

            if (review.Rating <= 2)
            {
                review.Sentiment = "Negative";
                TempData["ReviewMessage"] = "We're sorry to hear that. Our team will look into this.";
            }
            else if (review.Rating == 3)
            {
                review.Sentiment = "Neutral";
                TempData["ReviewMessage"] = "Thank you for your honest review.";
            }
            else
            {
                review.Sentiment = "Positive";
                TempData["ReviewMessage"] = "Thanks for your feedback! You've earned a loyalty reward.";
            }

            _context.ProductReviews.Add(review);
            _context.SaveChanges();

            return RedirectToAction("Details", "Product", new { id = review.ProductID });
        }
    }
}
