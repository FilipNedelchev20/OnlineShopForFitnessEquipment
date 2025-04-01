using FitnessEquipmentShop.Data.Models.Entities;
using FitnessEquipmentShop.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class CartController : Controller
{
    private readonly ICartService _cartService;
    private readonly UserManager<User> _userManager;
    public CartController(ICartService cartService, UserManager<User> userManager)
    {
        _cartService = cartService;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var cartItems = await _cartService.GetCartItemsAsync();
        return View(cartItems);
    }

    public async Task<IActionResult> Add(int productId)
    {
        // Ensure user is logged in, otherwise redirect to login.
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Login", "Account"); // or your login page
        }

        var userId = _userManager.GetUserId(User);
        await _cartService.AddToCartAsync(productId, userId);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Remove(int id)
    {
        await _cartService.RemoveFromCartAsync(id);
        return RedirectToAction("Index");
    }
}