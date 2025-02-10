using FitnessEquipmentShop.Data.Models.Entities;
using FitnessEquipmentShop.Data;
using Microsoft.AspNetCore.Mvc;

public class WishlistController : Controller
{
    private readonly ApplicationDbContext _context;

    public WishlistController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var wishlist = _context.Wishlist.ToList();
        return View(wishlist);
    }

    public IActionResult Add(int productId)
    {
        var product = _context.Products.Find(productId);
        if (product == null) return NotFound();

        var wishlistItem = new Wishlist { ProductId = productId };
        _context.Wishlist.Add(wishlistItem);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }
}
