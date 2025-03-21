using FitnessEquipmentShop.Data.Models.Entities;
using FitnessEquipmentShop.Services;
using FitnessEquipmentShop.Web.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
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
        return View();
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
    public async Task<IActionResult> PlaceOrder(FitnessEquipmentShop.Data.Models.Entities.Address address)
    {
        var userId = _userManager.GetUserId(User);
        try
        {
            await _orderService.PlaceOrderAsync(userId, address);
        }
        catch (Exception ex)
        {
            return RedirectToAction("Checkout");
        }
        return RedirectToAction("OrderConfirmation", new { orderId = 1 });
    }
}
