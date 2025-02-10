using FitnessEquipmentShop.Data.Models.Entities;
using FitnessEquipmentShop.Data;
using Microsoft.AspNetCore.Mvc;

public class ReviewController : Controller
{
    private readonly ApplicationDbContext _context;

    public ReviewController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index(int productId)
    {
        var reviews = _context.Reviews.Where(r => r.ProductId == productId).ToList();
        return View(reviews);
    }

    [HttpPost]
    public IActionResult Add(Review review)
    {
        if (ModelState.IsValid)
        {
            _context.Reviews.Add(review);
            _context.SaveChanges();
        }
        return RedirectToAction("Index", new { productId = review.ProductId });
    }
}
