using FitnessEquipmentShop.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class CartController : Controller
{
    private readonly ICartService _cartService;
    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    public async Task<IActionResult> Index()
    {
        var cartItems = await _cartService.GetCartItemsAsync();
        return View(cartItems);
    }

    public async Task<IActionResult> Add(int productId)
    {
        await _cartService.AddToCartAsync(productId);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Remove(int id)
    {
        await _cartService.RemoveFromCartAsync(id);
        return RedirectToAction("Index");
    }
}
