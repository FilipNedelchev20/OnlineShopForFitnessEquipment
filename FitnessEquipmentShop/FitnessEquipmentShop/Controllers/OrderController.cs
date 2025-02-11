using FitnessEquipmentShop.Data.Models.Entities;
using FitnessEquipmentShop.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FitnessEquipmentShop.Web.ViewModel;

[Authorize]
public class OrderController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;



    public OrderController(ApplicationDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
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
    [HttpPost]
    public IActionResult PlaceOrder(Address address)
    {
        var userId = _userManager.GetUserId(User);
        var cartItems = _context.CartItems
            .Include(c => c.Product)
            .Where(c => c.UserId == userId)
            .ToList();

        if (!cartItems.Any())
        {
            return RedirectToAction("Index", "Cart");
        }

        var order = new Order
        {
            UserId = userId,
            OrderDate = DateTime.Now,
            TotalAmount = cartItems.Sum(c => c.Product.Price * c.Quantity),
            OrderDetails = cartItems.Select(c => new OrderDetail
            {
                ProductId = c.ProductId,
                Quantity = c.Quantity,
                UnitPrice = c.Product.Price
            }).ToList(),
            Address = address
        };

        _context.Orders.Add(order);
        _context.CartItems.RemoveRange(cartItems);
        _context.SaveChanges();

        return RedirectToAction("OrderConfirmation", new { orderId = order.Id });
    }

}
