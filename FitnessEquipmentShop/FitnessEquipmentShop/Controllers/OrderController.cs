using FitnessEquipmentShop.Data.Models.Entities;
using FitnessEquipmentShop.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

[Authorize]
public class OrderController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager _userManager;
    public OrderController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var orders = _context.Orders.ToList();
        return View(orders);
    }

    public IActionResult Create()
    {
        var order = new Order { OrderDate = DateTime.Now };
        _context.Orders.Add(order);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }
    public IActionResult Checkout()
    {
        var userId = _userManager.GetUserId(User);
        var cartItems = _context.CartItems
            .Include(c => c.Product)
            .Where(c => c.UserId == userId)
            .ToList();

        if (!cartItems.Any())
        {
            return RedirectToAction("Index", "Cart"); // Redirect if cart is empty
        }

        var checkoutViewModel = new CheckoutViewModel
        {
            CartItems = cartItems,
            TotalPrice = cartItems.Sum(c => c.Product.Price * c.Quantity),
            Address = _context.Addresses.FirstOrDefault(a => a.UserId == userId) // Pre-fill address if exists
        };

        return View(checkoutViewModel);
    }

}
