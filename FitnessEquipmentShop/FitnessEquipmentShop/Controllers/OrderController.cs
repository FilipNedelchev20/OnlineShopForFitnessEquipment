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

    public async Task<IActionResult> Index()
    {
        var orders = await _context.Orders.ToListAsync();
        return View(orders);
    }

    public async Task<IActionResult> Create()
    {
        var order = new Order { OrderDate = DateTime.Now };
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> Checkout()
    {
        var userId = _userManager.GetUserId(User);
        var cartItems = await _context.CartItems
            .Include(c => c.Product)
            .Where(c => c.UserId == userId)
            .ToListAsync();

        if (!cartItems.Any())
        {
            return RedirectToAction("Index", "Cart"); 
        }

        var checkoutViewModel = new CheckoutViewModel
        {
            CartItems = cartItems,
            TotalPrice = cartItems.Sum(c => c.Product.Price * c.Quantity),
            Address = await _context.Addresses.FirstOrDefaultAsync(a => a.UserId == userId) 
        };

        return View(checkoutViewModel);
    }
    [HttpPost]
    public async Task<IActionResult> PlaceOrder(Address address)
    {
        var userId = _userManager.GetUserId(User);
        var cartItems = await _context.CartItems
            .Include(c => c.Product)
            .Where(c => c.UserId == userId)
            .ToListAsync();

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

        await _context.Orders.AddAsync(order);
        _context.CartItems.RemoveRange(cartItems);
        await _context.SaveChangesAsync();

        return RedirectToAction("OrderConfirmation", new { orderId = order.Id });
    }

}
