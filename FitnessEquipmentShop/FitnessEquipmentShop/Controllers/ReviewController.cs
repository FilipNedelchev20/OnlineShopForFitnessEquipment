using FitnessEquipmentShop.Services;
using FitnessEquipmentShop.Data.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using FitnessEquipmentShop.Web.ViewModel;

[Authorize]
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
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add([FromForm] ReviewViewModel review)
    {
        if (!User.Identity.IsAuthenticated)
        {
            return Json(new { success = false, message = "You must be logged in to leave a review." });
        }

        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, message = "Invalid review: " + string.Join("; ", errors) });
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var newReview = new Review
        {
            Id = review.Id,
            Comment = review.Comment,
            Rating = review.Rating,
            UserId = userId,
            ProductId = review.ProductId
        };

        try
        {
            await _reviewService.AddReviewAsync(newReview);

            var fullName = User.Identity.Name;
            return Json(new
            {
                success = true,
                comment = review.Comment,
                rating = review.Rating,
                fullName
            });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = $"Error: {ex.Message}" });
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var review = await _reviewService.GetReviewByIdAsync(id);
        if (review == null || (!User.IsInRole("Admin") && review.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier)))
        {
            return Unauthorized();
        }

        var model = new ReviewViewModel
        {
            Id = review.Id,
            Comment = review.Comment,
            Rating = review.Rating,
            ProductId = review.ProductId
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ReviewViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var review = await _reviewService.GetReviewByIdAsync(model.Id);
        if (review == null || (!User.IsInRole("Admin") && review.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier)))
        {
            return Unauthorized();
        }

        review.Comment = model.Comment;
        review.Rating = model.Rating;

        await _reviewService.UpdateReviewAsync(review);
        return RedirectToAction("Details", "Product", new { id = model.ProductId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var review = await _reviewService.GetReviewByIdAsync(id);
        if (review == null || (!User.IsInRole("Admin") && review.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier)))
        {
            return Unauthorized();
        }

        await _reviewService.DeleteReviewAsync(id);
        return RedirectToAction("Details", "Product", new { id = review.ProductId });
    }
}
