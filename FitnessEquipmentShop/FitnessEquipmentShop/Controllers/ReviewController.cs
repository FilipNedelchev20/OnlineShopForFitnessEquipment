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
    public async Task<IActionResult> Add(Review review)
    {
        if (ModelState.IsValid)
        {
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Index", new { productId = review.ProductId });
    }
}
