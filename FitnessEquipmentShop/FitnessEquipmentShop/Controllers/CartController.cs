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

    public IActionResult Add(int productId)
    {
        var product = _context.Products.Find(productId);
        if (product == null) return NotFound();

        var cartItem = new CartItem { ProductId = productId };
        _context.CartItems.Add(cartItem);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }

    public IActionResult Remove(int id)
    {
        var cartItem = _context.CartItems.Find(id);
        if (cartItem != null)
        {
            _context.CartItems.Remove(cartItem);
            _context.SaveChanges();
        }
        return RedirectToAction("Index");
    }
}
