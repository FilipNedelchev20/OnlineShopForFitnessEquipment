using FitnessEquipmentShop.Data.Models.Entities;
using FitnessEquipmentShop.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

[Authorize]
public class OrderController : Controller
{
    private readonly IOrderService _orderService;
    private readonly UserManager<User> _userManager;

    public OrderController(IOrderService orderService, UserManager<User> userManager)
    {
        _orderService = orderService;
        _userManager = userManager;
    }
    public async Task<IActionResult> Index()
    {
        var userId = _userManager.GetUserId(User);
        var orders = await _orderService.GetOrdersByUserIdAsync(userId);
        return View(orders);
    }
    public async Task<IActionResult> Checkout()
    {
        var userId = _userManager.GetUserId(User);
        var checkoutData = await _orderService.GetCheckoutDataAsync(userId);
        if (checkoutData == null)
        {
            return RedirectToAction("Index", "Cart");
        }
        return View(checkoutData);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PlaceOrder(Address address)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        if (!ModelState.IsValid)
        {
            return RedirectToAction("Checkout", "Order");
        }

        try
        {
            await _orderService.PlaceOrderAsync(userId, address);
            TempData["SuccessMessage"] = "✅ Your order has been placed successfully!";
            return RedirectToAction("OrderConfirmation");
        }
        catch (System.Exception ex)
        {
            TempData["ErrorMessage"] = $"❌ Order failed: {ex.Message}";
            return RedirectToAction("Checkout", "Order");
        }
    }

    [HttpGet]
    public IActionResult OrderConfirmation()
    {
        return View();
    }
}
