using FitnessEquipmentShop.Services;
using FitnessEquipmentShop.Data.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class ReviewController : Controller
{
    private readonly IReviewService _reviewService;
    public ReviewController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    public async Task<IActionResult> Index(int productId)
    {
        var reviews = await _reviewService.GetReviewsByProductIdAsync(productId);
        return View(reviews);
    }

    [HttpPost]
    public async Task<IActionResult> Add(Review review)
    {
        if (ModelState.IsValid)
        {
            await _reviewService.AddReviewAsync(review);
        }
        return RedirectToAction("Index", new { productId = review.ProductId });
    }
}
