using FitnessEquipmentShop.Data.Models.Entities;
using FitnessEquipmentShop.Services;
using FitnessEquipmentShop.Web.ViewModel;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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
        

        var userId = _userManager.GetUserId(User);
        await _cartService.AddToCartAsync(productId, userId);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Remove(int id)
    {
        await _cartService.RemoveFromCartAsync(id);
        return RedirectToAction("Index");
    }
    [HttpPost]
    public async Task<IActionResult> UpdateQuantities(List<CartItemViewModel> cartItems)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
     

        foreach (var item in cartItems)
        {
            await _cartService.UpdateQuantityAsync(userId, item.ProductId, item.Quantity);
        }

        return RedirectToAction("Checkout", "Order");
    }

}