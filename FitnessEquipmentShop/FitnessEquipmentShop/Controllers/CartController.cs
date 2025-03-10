using FitnessEquipmentShop.Data.Models.Entities;
using FitnessEquipmentShop.Data;
using Microsoft.AspNetCore.Mvc;

public class CartController : Controller
{
    private readonly ApplicationDbContext _context;

    public CartController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var cartItems = _context.CartItems.ToList();
        return View(cartItems);
    }

    public async Task<IActionResult> Add(int productId)
    {
        var product = await _context.Products.FindAsync(productId);
        if (product == null) return NotFound();

        var cartItem = new CartItem { ProductId = productId };
        await _context.CartItems.AddAsync(cartItem);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Remove(int id)
    {
        var cartItem = await _context.CartItems.FindAsync(id);
        if (cartItem != null)
        {
            _context.CartItems.Remove(cartItem);
           await _context.SaveChangesAsync();
        }
        return RedirectToAction("Index");
    }
}
