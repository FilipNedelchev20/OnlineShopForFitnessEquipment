using FitnessEquipmentShop.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

public class WishlistController : Controller
{
    private readonly IWishlistService _wishlistService;
    public WishlistController(IWishlistService wishlistService)
    {
        _wishlistService = wishlistService;
    }

    public async Task<IActionResult> Index()
    {
        var wishlist = await _wishlistService.GetWishlistItemsAsync();
        return View(wishlist);
    }

    public async Task<IActionResult> Add(int productId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Взема текущия UserId
        if (userId == null)
        {
            return Unauthorized(); // Или редирект към Login
        }

        await _wishlistService.AddToWishlistAsync(productId, userId);
        return RedirectToAction("Index");
    }


    public async Task<IActionResult> Remove(int id)
    {
        await _wishlistService.RemoveFromWishlistAsync(id);
        return RedirectToAction("Index");
    }
}